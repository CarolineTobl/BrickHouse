using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BrickHouse.Models
{
    public class PredictionService
    {
        private readonly IIntexRepository _repo;
        private readonly InferenceSession _session;

        public PredictionService(IIntexRepository repo)
        {
            _repo = repo;
            var modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Models", "final_decision_tree_model.onnx");
            _session = new InferenceSession(modelPath);
        }

        public int Predict(Order order, Customer customer)
        {
            var inputData = PrepareInputData(order, customer);
            var tensor = new DenseTensor<float>(inputData, new[] { 1, inputData.Length });
            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("float_input", tensor)
            };

            using var results = _session.Run(inputs);
            // Directly cast the result to int, assuming the output is a single int64 tensor with one element.
            var outputTensor = results.First().AsTensor<Int64>();
            int predictedClass = (int)outputTensor[0];
            return predictedClass;
        }

        private float[] PrepareInputData(Order order, Customer customer)
        {
            float[] data = new float[FeatureIndex.Indices.Count]; // Ensure the array size matches the number of features

            // Numeric and direct data
            data[FeatureIndex.Indices["time"]] = order.Time;
            data[FeatureIndex.Indices["amount"]] = (float)order.Amount;
            data[FeatureIndex.Indices["age"]] = (float)customer.Age;

            // Dummy coding for categorical variables
            AddOneHotEncodingForCategory(data, order.DayOfWeek, "day_of_week_", new List<string> { "Fri", "Mon", "Sat", "Sun", "Thu", "Tue", "Wed" });
            AddOneHotEncodingForCategory(data, order.TypeOfTransaction, "type_of_transaction_", new List<string> { "ATM", "Online", "POS" });
            AddOneHotEncodingForCategory(data, order.CountryOfTransaction, "country_of_transaction_", new List<string> { "China", "India", "Russia", "USA", "United Kingdom" });
            AddOneHotEncodingForCategory(data, order.ShippingAddress, "shipping_address_", new List<string> { "China", "India", "Russia", "USA", "United Kingdom" });
            AddOneHotEncodingForCategory(data, customer.CountryOfResidence, "country_of_residence_", new List<string> { "China", "India", "Russia", "USA", "United Kingdom" });

            // Bank, Card Type, and Entry Mode
            AddOneHotEncodingForCategory(data, order.Bank, "bank_", new List<string> { "Barclays", "HSBC", "Halifax", "Lloyds", "Metro", "Monzo", "RBS" });
            AddOneHotEncodingForCategory(data, order.TypeOfCard, "type_of_card_", new List<string> { "MasterCard", "Visa" });
            AddOneHotEncodingForCategory(data, order.EntryMode, "entry_mode_", new List<string> { "CVC", "PIN", "Tap" });

            // Gender
            AddOneHotEncodingForCategory(data, customer.Gender, "gender_", new List<string> { "F", "M" });

            // Country Mismatches
            data[FeatureIndex.Indices["mismatch_trans_ship"]] = order.CountryOfTransaction != order.ShippingAddress ? 1 : 0;
            data[FeatureIndex.Indices["mismatch_trans_res"]] = order.CountryOfTransaction != customer.CountryOfResidence ? 1 : 0;
            data[FeatureIndex.Indices["mismatch_ship_res"]] = order.ShippingAddress != customer.CountryOfResidence ? 1 : 0;

            return data;
        }

        private void AddOneHotEncodingForCategory(float[] data, string actualValue, string prefix, List<string> values)
        {
            foreach (var value in values)
            {
                data[FeatureIndex.Indices[prefix + value]] = actualValue == value ? 1f : 0f;
            }
        }

        public static class FeatureIndex
        {
            public static readonly Dictionary<string, int> Indices = new Dictionary<string, int>
            {
                {"time", 0}, {"amount", 1}, {"age", 2},
                {"type_of_transaction_ATM", 3}, {"type_of_transaction_Online", 4}, {"type_of_transaction_POS", 5},
                {"country_of_transaction_China", 6}, {"country_of_transaction_India", 7}, {"country_of_transaction_Russia", 8},
                {"country_of_transaction_USA", 9}, {"country_of_transaction_United Kingdom", 10},
                {"shipping_address_China", 11}, {"shipping_address_India", 12}, {"shipping_address_Russia", 13},
                {"shipping_address_USA", 14}, {"shipping_address_United Kingdom", 15},
                {"bank_Barclays", 16}, {"bank_HSBC", 17}, {"bank_Halifax", 18},
                {"bank_Lloyds", 19}, {"bank_Metro", 20}, {"bank_Monzo", 21}, {"bank_RBS", 22},
                {"type_of_card_MasterCard", 23}, {"type_of_card_Visa", 24},
                {"entry_mode_CVC", 25}, {"entry_mode_PIN", 26}, {"entry_mode_Tap", 27},
                {"day_of_week_Fri", 28}, {"day_of_week_Mon", 29}, {"day_of_week_Sat", 30},
                {"day_of_week_Sun", 31}, {"day_of_week_Thu", 32}, {"day_of_week_Tue", 33}, {"day_of_week_Wed", 34},
                {"gender_F", 35}, {"gender_M", 36},
                {"country_of_residence_China", 37}, {"country_of_residence_India", 38}, {"country_of_residence_Russia", 39},
                {"country_of_residence_USA", 40}, {"country_of_residence_United Kingdom", 41},
                {"mismatch_trans_ship", 42}, {"mismatch_trans_res", 43}, {"mismatch_ship_res", 44}
            };
        }
    }
}