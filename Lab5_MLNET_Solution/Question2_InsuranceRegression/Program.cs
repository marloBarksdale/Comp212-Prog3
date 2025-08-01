using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace COMP212_Lab5_Question2
{
    // Data model for insurance dataset
    public class InsuranceData
    {
        [LoadColumn(0)]
        public float Age { get; set; }

        [LoadColumn(1)]
        public string Sex { get; set; }

        [LoadColumn(2)]
        public float Bmi { get; set; }

        [LoadColumn(3)]
        public float Children { get; set; }

        [LoadColumn(4)]
        public string Smoker { get; set; }

        [LoadColumn(5)]
        public string Region { get; set; }

        [LoadColumn(6)]
        [ColumnName("Label")]
        public float Charges { get; set; }
    }

    // Prediction output class
    public class InsurancePrediction
    {
        [ColumnName("Score")]
        public float PredictedCost { get; set; }
    }

    class Program
    {
        private static readonly MLContext mlContext = new MLContext(seed: 0);

        static void Main(string[] args)
        {
            Console.WriteLine("=== Question 2: Medical Cost Prediction (Regression) ===\n");

            // 2.1 Generate cost prediction regression model
            var model = BuildRegressionModel();

            // 2.2 Consume the generated model
            TestRegressionModel(model);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static ITransformer BuildRegressionModel()
        {
            try
            {
                string dataPath = "insurance.csv";

                Console.WriteLine("Loading insurance data...");
                IDataView dataView = mlContext.Data.LoadFromTextFile<InsuranceData>(
                    dataPath, hasHeader: true, separatorChar: ',');

                Console.WriteLine($"Loaded {mlContext.Data.CreateEnumerable<InsuranceData>(dataView, false).Count()} records");

                // Split the dataset into training and testing sets (80/20)
                var splitDataView = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);

                // Build the training pipeline for regression
                var estimator = mlContext.Transforms.Text.FeaturizeText(
                        outputColumnName: "SexFeaturized",
                        inputColumnName: nameof(InsuranceData.Sex))
                    .Append(mlContext.Transforms.Text.FeaturizeText(
                        outputColumnName: "SmokerFeaturized",
                        inputColumnName: nameof(InsuranceData.Smoker)))
                    .Append(mlContext.Transforms.Text.FeaturizeText(
                        outputColumnName: "RegionFeaturized",
                        inputColumnName: nameof(InsuranceData.Region)))
                    .Append(mlContext.Transforms.Concatenate("Features",
                        nameof(InsuranceData.Age), nameof(InsuranceData.Bmi),
                        nameof(InsuranceData.Children), "SexFeaturized",
                        "SmokerFeaturized", "RegionFeaturized"))
                    .Append(mlContext.Regression.Trainers.Sdca(
                        labelColumnName: "Label", featureColumnName: "Features"));

                Console.WriteLine("Training regression model using SDCA algorithm...");
                ITransformer model = estimator.Fit(splitDataView.TrainSet);

                // Evaluate the model
                Console.WriteLine("Evaluating regression model performance...");
                IDataView predictions = model.Transform(splitDataView.TestSet);
                var metrics = mlContext.Regression.Evaluate(predictions, "Label");

                Console.WriteLine($"\nRegression Model Performance Metrics:");
                Console.WriteLine($"Mean Absolute Error: ${metrics.MeanAbsoluteError:F2}");
                Console.WriteLine($"Mean Squared Error: {metrics.MeanSquaredError:F2}");
                Console.WriteLine($"Root Mean Squared Error: ${metrics.RootMeanSquaredError:F2}");
                Console.WriteLine($"R-Squared (coefficient of determination): {metrics.RSquared:F3}");
                Console.WriteLine($"Loss Function: {metrics.LossFunction:F2}");

                // Save the model
                mlContext.Model.Save(model, dataView.Schema, "insurance_cost_regression_model.zip");
                Console.WriteLine("\nModel saved as 'insurance_cost_regression_model.zip'");

                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error building regression model: {ex.Message}");
                throw;
            }
        }

        static void TestRegressionModel(ITransformer model)
        {
            try
            {
                // Create prediction engine
                var predictionEngine = mlContext.Model.CreatePredictionEngine<InsuranceData, InsurancePrediction>(model);

                Console.WriteLine("\n=== Testing Regression Model with Sample Data ===");

                // Test case 1: Young non-smoker
                var youngNonSmoker = new InsuranceData()
                {
                    Age = 25,
                    Sex = "female",
                    Bmi = 22.5f,
                    Children = 0,
                    Smoker = "no",
                    Region = "southwest"
                };

                var prediction1 = predictionEngine.Predict(youngNonSmoker);
                Console.WriteLine($"\nYoung Non-Smoker (25F, BMI 22.5, 0 children):");
                Console.WriteLine($"Predicted Medical Cost: ${prediction1.PredictedCost:F2}");

                // Test case 2: Middle-aged smoker
                var middleAgedSmoker = new InsuranceData()
                {
                    Age = 45,
                    Sex = "male",
                    Bmi = 28.0f,
                    Children = 2,
                    Smoker = "yes",
                    Region = "southeast"
                };

                var prediction2 = predictionEngine.Predict(middleAgedSmoker);
                Console.WriteLine($"\nMiddle-Aged Smoker (45M, BMI 28.0, 2 children):");
                Console.WriteLine($"Predicted Medical Cost: ${prediction2.PredictedCost:F2}");

                // Test case 3: Older adult with high BMI
                var olderAdult = new InsuranceData()
                {
                    Age = 55,
                    Sex = "female",
                    Bmi = 32.0f,
                    Children = 3,
                    Smoker = "no",
                    Region = "northeast"
                };

                var prediction3 = predictionEngine.Predict(olderAdult);
                Console.WriteLine($"\nOlder Adult (55F, BMI 32.0, 3 children):");
                Console.WriteLine($"Predicted Medical Cost: ${prediction3.PredictedCost:F2}");

                // Interactive prediction
                Console.WriteLine("\n=== Interactive Cost Prediction ===");
                Console.WriteLine("Enter patient details for cost prediction:");

                var interactivePatient = new InsuranceData();

                // Age with ACTUAL range from dataset (18-64)
                interactivePatient.Age = GetValidatedFloat(
                    "Age",
                    18f,
                    64f,
                    "Please enter an age between 18 and 64"
                );

                // Sex with ACTUAL values from dataset
                string[] validSex = { "female", "male" };
                interactivePatient.Sex = GetValidatedInput(
                    "Sex",
                    validSex,
                    "Female/Male"
                );

                // BMI with ACTUAL range from dataset (16.0-53.1)
                interactivePatient.Bmi = GetValidatedFloat(
                    "BMI",
                    16.0f,
                    53.1f,
                    "Please enter a BMI between 16.0 and 53.1"
                );

                // Children with ACTUAL range from dataset (0-5)
                interactivePatient.Children = GetValidatedFloat(
                    "Number of children",
                    0f,
                    5f,
                    "Please enter a number between 0 and 5"
                );

                // Smoker with ACTUAL values from dataset
                string[] validSmoker = { "yes", "no" };
                interactivePatient.Smoker = GetValidatedInput(
                    "Smoker",
                    validSmoker,
                    "Yes/No"
                );

                // Region with ACTUAL values from dataset
                string[] validRegions = { "southwest", "southeast", "northwest", "northeast" };
                interactivePatient.Region = GetValidatedInput(
                    "Region",
                    validRegions,
                    "Southwest/Southeast/Northwest/Northeast"
                );

                var interactivePrediction = predictionEngine.Predict(interactivePatient);
                Console.WriteLine($"\nPredicted Medical Cost for your input: ${interactivePrediction.PredictedCost:F2}");

                // Show feature importance analysis
                Console.WriteLine("\n=== Feature Impact Analysis ===");
                Console.WriteLine("Based on the model, the key factors affecting medical costs are:");
                Console.WriteLine("1. Smoking status (smokers typically have much higher costs)");
                Console.WriteLine("2. Age (older patients generally have higher costs)");
                Console.WriteLine("3. BMI (higher BMI can correlate with higher costs)");
                Console.WriteLine("4. Number of children (may affect family medical costs)");
                Console.WriteLine("5. Geographic region (regional cost variations)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error testing regression model: {ex.Message}");
            }
        }

        // Helper methods for input validation
        static string GetValidatedInput(string fieldName, string[] validOptions, string optionsDisplay)
        {
            while (true)
            {
                Console.WriteLine($"\n{fieldName} (Valid options: {optionsDisplay}):");
                Console.Write($"Enter {fieldName.ToLower()}: ");
                string input = Console.ReadLine()?.Trim().ToLower() ?? "";

                if (validOptions.Contains(input))
                {
                    // Return the exact case from the dataset (all lowercase for insurance data)
                    return input;
                }

                Console.WriteLine($"Invalid input. Please enter one of: {string.Join(", ", validOptions)}");
            }
        }

        static float GetValidatedFloat(string fieldName, float min, float max, string errorMessage)
        {
            while (true)
            {
                Console.WriteLine($"\n{fieldName} ({min}-{max}):");
                Console.Write($"Enter {fieldName.ToLower()}: ");
                string input = Console.ReadLine()?.Trim() ?? "";

                if (float.TryParse(input, out float value) && value >= min && value <= max)
                {
                    return value;
                }

                Console.WriteLine($"Invalid input. {errorMessage}");
            }
        }
    }
}
