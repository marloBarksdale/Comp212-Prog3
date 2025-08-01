using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace COMP212_Lab5_Question3
{
    // 3.1 Student class for the dataset
    public class StudentData
    {
        [LoadColumn(0)]
        public float STG { get; set; } // Study time for goal object materials

        [LoadColumn(1)]
        public float SCG { get; set; } // Repetition number for goal object materials

        [LoadColumn(2)]
        public float STR { get; set; } // Study time for related objects

        [LoadColumn(3)]
        public float LPR { get; set; } // Exam performance for related objects

        [LoadColumn(4)]
        public float PEG { get; set; } // Exam performance for goal objects

        [LoadColumn(5)]
        public string UNS { get; set; } // Knowledge level of user (note: has space in CSV)
    }

    // 3.1 Cluster Prediction class
    public class ClusterPrediction
    {
        [ColumnName("PredictedLabel")]
        public uint PredictedClusterId { get; set; }

        [ColumnName("Score")]
        public float[] Distances { get; set; }
    }

    class Program
    {
        private static readonly MLContext mlContext = new MLContext(seed: 0);

        static void Main(string[] args)
        {
            Console.WriteLine("=== Question 3: Student Knowledge Level Prediction (Clustering) ===\n");

            Console.WriteLine("Dataset Attribute Information:");
            Console.WriteLine("STG - The degree of study time for goal object materials");
            Console.WriteLine("SCG - The degree of repetition number of user for goal object materials");
            Console.WriteLine("STR - The degree of study time of user for related objects with goal object");
            Console.WriteLine("LPR - The exam performance of user for related objects with goal object");
            Console.WriteLine("PEG - The exam performance of user for goal objects");
            Console.WriteLine("UNS - The knowledge level of user\n");

            // Build clustering model
            var model = BuildClusteringModel();

            // Test the model
            TestClusteringModel(model);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static ITransformer BuildClusteringModel()
        {
            try
            {
                string dataPath = "Student.csv";

                Console.WriteLine("Loading student data...");
                IDataView dataView = mlContext.Data.LoadFromTextFile<StudentData>(
                    dataPath, hasHeader: true, separatorChar: ',');

                Console.WriteLine($"Loaded {mlContext.Data.CreateEnumerable<StudentData>(dataView, false).Count()} student records");

                // 3.2 Customize options for K-Means
                var options = new Microsoft.ML.Trainers.KMeansTrainer.Options()
                {
                    NumberOfClusters = 3, // Based on typical knowledge levels: Low, Medium, High
                    OptimizationTolerance = 1e-6f,
                    MaximumNumberOfIterations = 100,
                    InitializationAlgorithm = Microsoft.ML.Trainers.KMeansTrainer.InitializationAlgorithm.KMeansPlusPlus
                };

                // 3.3 Create the pipeline
                var estimator = mlContext.Transforms.Concatenate("Features",
                        nameof(StudentData.STG), nameof(StudentData.SCG),
                        nameof(StudentData.STR), nameof(StudentData.LPR),
                        nameof(StudentData.PEG))
                    .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                    .Append(mlContext.Clustering.Trainers.KMeans(options));

                Console.WriteLine("Training K-Means clustering model...");
                Console.WriteLine($"Number of clusters: {options.NumberOfClusters}");
                Console.WriteLine($"Initialization algorithm: {options.InitializationAlgorithm}");
                Console.WriteLine($"Maximum iterations: {options.MaximumNumberOfIterations}");

                ITransformer model = estimator.Fit(dataView);

                // Evaluate clustering model
                Console.WriteLine("Evaluating clustering model...");
                IDataView predictions = model.Transform(dataView);
                var metrics = mlContext.Clustering.Evaluate(predictions, scoreColumnName: "Score", featureColumnName: "Features");

                Console.WriteLine($"\nClustering Model Performance Metrics:");
                Console.WriteLine($"Average Distance: {metrics.AverageDistance:F3}");
                Console.WriteLine($"Davies Bouldin Index: {metrics.DaviesBouldinIndex:F3}");

                // Analyze cluster distribution
                var clusterData = mlContext.Data.CreateEnumerable<ClusterPrediction>(predictions, false).ToArray();
                var clusterCounts = clusterData.GroupBy(x => x.PredictedClusterId)
                                              .Select(g => new { ClusterId = g.Key, Count = g.Count() })
                                              .OrderBy(x => x.ClusterId);

                Console.WriteLine($"\nCluster Distribution:");
                foreach (var cluster in clusterCounts)
                {
                    Console.WriteLine($"Cluster {cluster.ClusterId}: {cluster.Count} students ({(double)cluster.Count / clusterData.Length:P1})");
                }

                // Save the model
                mlContext.Model.Save(model, dataView.Schema, "student_clustering_model.zip");
                Console.WriteLine("\nModel saved as 'student_clustering_model.zip'");

                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error building clustering model: {ex.Message}");
                throw;
            }
        }

        static void TestClusteringModel(ITransformer model)
        {
            try
            {
                // 3.4 Create the prediction engine from the model and perform the prediction
                var predictionEngine = mlContext.Model.CreatePredictionEngine<StudentData, ClusterPrediction>(model);

                Console.WriteLine("\n=== Testing Clustering Model with Sample Data ===");

                // Test case 1: High-performing student
                var highPerformingStudent = new StudentData()
                {
                    STG = 0.8f,  // High study time for goal materials
                    SCG = 0.7f,  // Good repetition rate
                    STR = 0.6f,  // Moderate study time for related materials
                    LPR = 0.85f, // High performance on related exams
                    PEG = 0.9f   // Excellent performance on goal exams
                };

                var prediction1 = predictionEngine.Predict(highPerformingStudent);
                Console.WriteLine($"\nHigh-Performing Student:");
                Console.WriteLine($"Predicted Cluster: {prediction1.PredictedClusterId}");
                if (prediction1.Distances != null && prediction1.Distances.Length > 0)
                {
                    Console.WriteLine($"Distances to all clusters: [{string.Join(", ", prediction1.Distances.Select(d => d.ToString("F3")))}]");
                }

                // Test case 2: Low-performing student
                var lowPerformingStudent = new StudentData()
                {
                    STG = 0.1f,  // Low study time
                    SCG = 0.05f, // Minimal repetition
                    STR = 0.1f,  // Low study time for related materials
                    LPR = 0.2f,  // Poor performance on related exams
                    PEG = 0.15f  // Poor performance on goal exams
                };

                var prediction2 = predictionEngine.Predict(lowPerformingStudent);
                Console.WriteLine($"\nLow-Performing Student:");
                Console.WriteLine($"Predicted Cluster: {prediction2.PredictedClusterId}");
                if (prediction2.Distances != null && prediction2.Distances.Length > 0)
                {
                    Console.WriteLine($"Distances to all clusters: [{string.Join(", ", prediction2.Distances.Select(d => d.ToString("F3")))}]");
                }

                // Test case 3: Average student
                var averageStudent = new StudentData()
                {
                    STG = 0.4f,  // Moderate study time
                    SCG = 0.3f,  // Average repetition
                    STR = 0.35f, // Average study time for related materials
                    LPR = 0.45f, // Average performance on related exams
                    PEG = 0.5f   // Average performance on goal exams
                };

                var prediction3 = predictionEngine.Predict(averageStudent);
                Console.WriteLine($"\nAverage Student:");
                Console.WriteLine($"Predicted Cluster: {prediction3.PredictedClusterId}");
                if (prediction3.Distances != null && prediction3.Distances.Length > 0)
                {
                    Console.WriteLine($"Distances to all clusters: [{string.Join(", ", prediction3.Distances.Select(d => d.ToString("F3")))}]");
                }

                // Interactive prediction
                Console.WriteLine("\n=== Interactive Student Knowledge Level Prediction ===");
                Console.WriteLine("Enter student performance data based on actual dataset ranges:");
                Console.WriteLine("Note: All values should be decimal numbers between 0.0 and 1.0 (based on actual data)");
                Console.WriteLine("\nDataset Knowledge Levels: very_low, Low, Middle, High");

                var interactiveStudent = new StudentData();

                // STG with validation and explanation (actual range: 0.0-0.99)
                Console.WriteLine("\nSTG = Study time for goal object materials");
                interactiveStudent.STG = GetValidatedFloat(
                    "STG (Study time for goal materials)",
                    0.0f,
                    1.0f,
                    "Please enter a value between 0.0 and 1.0 (actual dataset range: 0.0-0.99)"
                );

                // SCG with validation and explanation (actual range: 0.0-0.9)
                Console.WriteLine("\nSCG = Repetition number for goal object materials");
                interactiveStudent.SCG = GetValidatedFloat(
                    "SCG (Repetition for goal materials)",
                    0.0f,
                    1.0f,
                    "Please enter a value between 0.0 and 1.0 (actual dataset range: 0.0-0.9)"
                );

                // STR with validation and explanation
                Console.WriteLine("\nSTR = Study time for related objects with goal object");
                interactiveStudent.STR = GetValidatedFloat(
                    "STR (Study time for related materials)",
                    0.0f,
                    1.0f,
                    "Please enter a value between 0.0 and 1.0"
                );

                // LPR with validation and explanation
                Console.WriteLine("\nLPR = Exam performance for related objects with goal object");
                interactiveStudent.LPR = GetValidatedFloat(
                    "LPR (Performance on related exams)",
                    0.0f,
                    1.0f,
                    "Please enter a value between 0.0 and 1.0"
                );

                // PEG with validation and explanation (actual range: 0.0-0.93)
                Console.WriteLine("\nPEG = Exam performance for goal objects");
                interactiveStudent.PEG = GetValidatedFloat(
                    "PEG (Performance on goal exams)",
                    0.0f,
                    1.0f,
                    "Please enter a value between 0.0 and 1.0 (actual dataset range: 0.0-0.93)"
                );

                var interactivePrediction = predictionEngine.Predict(interactiveStudent);
                Console.WriteLine($"\nPrediction for your input:");
                Console.WriteLine($"Predicted Knowledge Level Cluster: {interactivePrediction.PredictedClusterId}");

                if (interactivePrediction.Distances != null && interactivePrediction.Distances.Length > 0)
                {
                    Console.WriteLine($"Distances to all clusters: [{string.Join(", ", interactivePrediction.Distances.Select(d => d.ToString("F3")))}]");

                    // Show which cluster they're closest to
                    float minDistance = interactivePrediction.Distances.Min();
                    int closestCluster = Array.IndexOf(interactivePrediction.Distances, minDistance);
                    Console.WriteLine($"You are closest to cluster {closestCluster} (distance: {minDistance:F3})");
                }

                // Interpret clusters based on actual dataset values
                Console.WriteLine($"\nCluster Interpretation:");
                Console.WriteLine($"Based on the dataset, the knowledge levels are: very_low, Low, Middle, High");
                Console.WriteLine($"Your predicted cluster is: {interactivePrediction.PredictedClusterId}");
                Console.WriteLine($"The clustering algorithm has grouped students with similar learning patterns.");
                if (interactivePrediction.Distances != null && interactivePrediction.Distances.Length > 0)
                {
                    Console.WriteLine($"Lower distances in the distance array indicate closer similarity to that cluster.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error testing clustering model: {ex.Message}");
            }
        }

        // Helper method to analyze cluster characteristics
        static void AnalyzeClusterCharacteristics()
        {
            Console.WriteLine("\n=== Cluster Analysis ===");
            Console.WriteLine("The dataset contains students with these actual knowledge levels:");
            Console.WriteLine("• very_low - Students with very low knowledge level");
            Console.WriteLine("• Low - Students with low knowledge level");
            Console.WriteLine("• Middle - Students with middle knowledge level");
            Console.WriteLine("• High - Students with high knowledge level");
            Console.WriteLine("\nThe K-Means clustering algorithm groups students based on similarity in:");
            Console.WriteLine("• STG: Study time patterns for goal materials (0.0-0.99 range)");
            Console.WriteLine("• SCG: Practice/repetition habits for goal materials (0.0-0.9 range)");
            Console.WriteLine("• STR: Study time for related materials");
            Console.WriteLine("• LPR: Performance on related exams");
            Console.WriteLine("• PEG: Performance on goal exams (0.0-0.93 range)");
        }

        // Helper methods for input validation
        static float GetValidatedFloat(string fieldName, float min, float max, string errorMessage)
        {
            while (true)
            {
                Console.WriteLine($"\n{fieldName} ({min}-{max}):");
                Console.Write($"Enter value: ");
                string input = Console.ReadLine()?.Trim() ?? "";

                if (float.TryParse(input, out float value) && value >= min && value <= max)
                {
                    return value;
                }

                Console.WriteLine($"Invalid input. {errorMessage}");
                Console.WriteLine($"Example valid inputs: {min}, {(min + max) / 2:F1}, {max}");
            }
        }
    }
}