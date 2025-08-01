using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace COMP212_Lab5_Question1
{
    // Data model for AI assistant usage dataset
    public class AIUsageData
    {
        [LoadColumn(1)]
        public string StudentLevel { get; set; }

        [LoadColumn(2)]
        public string Discipline { get; set; }

        [LoadColumn(4)]
        public float SessionLengthMin { get; set; }

        [LoadColumn(5)]
        public float TotalPrompts { get; set; }

        [LoadColumn(6)]
        public string TaskType { get; set; }

        [LoadColumn(7)]
        public float AI_AssistanceLevel { get; set; }

        [LoadColumn(8)]
        public string FinalOutcome { get; set; }

        [LoadColumn(9)]
        [ColumnName("Label")]
        public bool UsedAgain { get; set; }

        [LoadColumn(10)]
        public float SatisfactionRating { get; set; }
    }

    // Prediction output class
    public class AIUsagePrediction
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }

        public float Probability { get; set; }

        public float Score { get; set; }
    }

    class Program
    {
        private static readonly MLContext mlContext = new MLContext(seed: 0);

        static void Main(string[] args)
        {
            Console.WriteLine("=== Question 1: AI Assistant Usage Prediction (Classification) ===\n");

            // 1.1 Generate classification model
            var model = BuildClassificationModel();

            // 1.2 Consume the generated model
            TestClassificationModel(model);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static ITransformer BuildClassificationModel()
        {
            try
            {
                string dataPath = "ai_assistant_usage_student_life.csv";

                Console.WriteLine("Loading AI assistant usage data...");
                IDataView dataView = mlContext.Data.LoadFromTextFile<AIUsageData>(
                    dataPath, hasHeader: true, separatorChar: ',');

                Console.WriteLine($"Loaded {mlContext.Data.CreateEnumerable<AIUsageData>(dataView, false).Count()} records");

                // Split the dataset into training and testing sets (80/20)
                var splitDataView = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);

                // Build the training pipeline
                var estimator = mlContext.Transforms.Text.FeaturizeText(
                        outputColumnName: "StudentLevelFeaturized",
                        inputColumnName: nameof(AIUsageData.StudentLevel))
                    .Append(mlContext.Transforms.Text.FeaturizeText(
                        outputColumnName: "DisciplineFeaturized",
                        inputColumnName: nameof(AIUsageData.Discipline)))
                    .Append(mlContext.Transforms.Text.FeaturizeText(
                        outputColumnName: "TaskTypeFeaturized",
                        inputColumnName: nameof(AIUsageData.TaskType)))
                    .Append(mlContext.Transforms.Text.FeaturizeText(
                        outputColumnName: "FinalOutcomeFeaturized",
                        inputColumnName: nameof(AIUsageData.FinalOutcome)))
                    .Append(mlContext.Transforms.Concatenate("Features",
                        "StudentLevelFeaturized", "DisciplineFeaturized", "TaskTypeFeaturized",
                        "FinalOutcomeFeaturized", nameof(AIUsageData.SessionLengthMin),
                        nameof(AIUsageData.TotalPrompts), nameof(AIUsageData.AI_AssistanceLevel),
                        nameof(AIUsageData.SatisfactionRating)))
                    .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                        labelColumnName: "Label", featureColumnName: "Features"));

                Console.WriteLine("Training binary classification model...");
                ITransformer model = estimator.Fit(splitDataView.TrainSet);

                // Evaluate the model
                Console.WriteLine("Evaluating model performance...");
                IDataView predictions = model.Transform(splitDataView.TestSet);
                var metrics = mlContext.BinaryClassification.Evaluate(predictions, "Label");

                Console.WriteLine($"\nModel Performance Metrics:");
                Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
                Console.WriteLine($"Area Under ROC Curve: {metrics.AreaUnderRocCurve:F3}");
                Console.WriteLine($"Area Under Precision-Recall Curve: {metrics.AreaUnderPrecisionRecallCurve:F3}");
                Console.WriteLine($"F1 Score: {metrics.F1Score:F3}");
                Console.WriteLine($"Positive Precision: {metrics.PositivePrecision:F3}");
                Console.WriteLine($"Positive Recall: {metrics.PositiveRecall:F3}");

                // Save the model
                mlContext.Model.Save(model, dataView.Schema, "ai_usage_classification_model.zip");
                Console.WriteLine("\nModel saved as 'ai_usage_classification_model.zip'");

                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error building classification model: {ex.Message}");
                throw;
            }
        }

        static void TestClassificationModel(ITransformer model)
        {
            try
            {
                // Create prediction engine
                var predictionEngine = mlContext.Model.CreatePredictionEngine<AIUsageData, AIUsagePrediction>(model);

                Console.WriteLine("\n=== Testing Model with Sample Data ===");

                // Test case 1: High engagement student
                var highEngagementStudent = new AIUsageData()
                {
                    StudentLevel = "Undergraduate",
                    Discipline = "Computer Science",
                    SessionLengthMin = 45.0f,
                    TotalPrompts = 15,
                    TaskType = "Assignment",
                    AI_AssistanceLevel = 4,
                    FinalOutcome = "Completed",
                    SatisfactionRating = 4.5f
                };

                var prediction1 = predictionEngine.Predict(highEngagementStudent);
                Console.WriteLine($"\nHigh Engagement Student:");
                Console.WriteLine($"Will use AI assistant again: {prediction1.Prediction}");
                Console.WriteLine($"Confidence: {prediction1.Probability:P2}");

                // Test case 2: Low engagement student
                var lowEngagementStudent = new AIUsageData()
                {
                    StudentLevel = "Graduate",
                    Discipline = "Philosophy",
                    SessionLengthMin = 10.0f,
                    TotalPrompts = 3,
                    TaskType = "Research",
                    AI_AssistanceLevel = 1,
                    FinalOutcome = "Abandoned",
                    SatisfactionRating = 2.0f
                };

                var prediction2 = predictionEngine.Predict(lowEngagementStudent);
                Console.WriteLine($"\nLow Engagement Student:");
                Console.WriteLine($"Will use AI assistant again: {prediction2.Prediction}");
                Console.WriteLine($"Confidence: {prediction2.Probability:P2}");

                // Interactive prediction
                Console.WriteLine("\n=== Interactive Prediction ===");
                Console.WriteLine("Enter student details for prediction:");

                var interactiveStudent = new AIUsageData();

                // Student Level with ACTUAL values from dataset
                string[] validLevels = { "undergraduate", "graduate", "high school" };
                interactiveStudent.StudentLevel = GetValidatedInput(
                    "Student Level",
                    validLevels,
                    "Undergraduate/Graduate/High School"
                );

                // Discipline with ACTUAL values from dataset
                string[] validDisciplines = { "computer science", "psychology", "business", "biology", "math", "history", "engineering" };
                interactiveStudent.Discipline = GetValidatedInput(
                    "Discipline",
                    validDisciplines,
                    "Computer Science/Psychology/Business/Biology/Math/History/Engineering"
                );

                // Session Length with validation (reasonable range based on data)
                interactiveStudent.SessionLengthMin = GetValidatedFloat(
                    "Session Length (minutes)",
                    1f,
                    300f,
                    "Please enter a value between 1 and 300 minutes"
                );

                // Total Prompts with validation
                interactiveStudent.TotalPrompts = GetValidatedFloat(
                    "Total Prompts",
                    1f,
                    100f,
                    "Please enter a value between 1 and 100"
                );

                // Task Type with ACTUAL values from dataset
                string[] validTaskTypes = { "studying", "coding", "writing", "brainstorming", "homework help", "research" };
                interactiveStudent.TaskType = GetValidatedInput(
                    "Task Type",
                    validTaskTypes,
                    "Studying/Coding/Writing/Brainstorming/Homework Help/Research"
                );

                // AI Assistance Level with validation
                interactiveStudent.AI_AssistanceLevel = GetValidatedFloat(
                    "AI Assistance Level",
                    1f,
                    5f,
                    "Please enter a value between 1 and 5"
                );

                // Final Outcome with ACTUAL values from dataset
                string[] validOutcomes = { "assignment completed", "idea drafted", "confused", "gave up" };
                interactiveStudent.FinalOutcome = GetValidatedInput(
                    "Final Outcome",
                    validOutcomes,
                    "Assignment Completed/Idea Drafted/Confused/Gave Up"
                );

                // Satisfaction Rating with validation
                interactiveStudent.SatisfactionRating = GetValidatedFloat(
                    "Satisfaction Rating",
                    1f,
                    5f,
                    "Please enter a value between 1.0 and 5.0"
                );

                var interactivePrediction = predictionEngine.Predict(interactiveStudent);
                Console.WriteLine($"\nPrediction for your input:");
                Console.WriteLine($"Will use AI assistant again: {interactivePrediction.Prediction}");
                Console.WriteLine($"Confidence: {interactivePrediction.Probability:P2}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error testing classification model: {ex.Message}");
            }
        }

        // Helper methods for input validation
        static string GetValidatedInput(string fieldName, string[] validOptions, string optionsDisplay)
        {
            // Create a mapping of lowercase to proper case for the actual dataset values
            var properCaseMap = new Dictionary<string, string>
            {
                // Student Levels
                {"undergraduate", "Undergraduate"},
                {"graduate", "Graduate"},
                {"high school", "High School"},
                
                // Disciplines
                {"computer science", "Computer Science"},
                {"psychology", "Psychology"},
                {"business", "Business"},
                {"biology", "Biology"},
                {"math", "Math"},
                {"history", "History"},
                {"engineering", "Engineering"},
                
                // Task Types
                {"studying", "Studying"},
                {"coding", "Coding"},
                {"writing", "Writing"},
                {"brainstorming", "Brainstorming"},
                {"homework help", "Homework Help"},
                {"research", "Research"},
                
                // Final Outcomes
                {"assignment completed", "Assignment Completed"},
                {"idea drafted", "Idea Drafted"},
                {"confused", "Confused"},
                {"gave up", "Gave Up"}
            };

            while (true)
            {
                Console.WriteLine($"\n{fieldName} (Valid options: {optionsDisplay}):");
                Console.Write($"Enter {fieldName.ToLower()}: ");
                string input = Console.ReadLine()?.Trim().ToLower() ?? "";

                if (validOptions.Contains(input))
                {
                    // Return with proper capitalization from dataset
                    return properCaseMap.ContainsKey(input) ? properCaseMap[input] : input;
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
