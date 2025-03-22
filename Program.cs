using System;
using System.Diagnostics;
using System.Linq;
using System.IO;
using Range = Microsoft.ML.Probabilistic.Models.Range;
using Microsoft.ML.Probabilistic.Models;
using Microsoft.ML.Probabilistic.Distributions;
using Microsoft.ML.Probabilistic.Algorithms;
using Microsoft.ML.Probabilistic.Math;

class MotifFinder
{
    static readonly char[] Nucleotides = { 'A', 'C', 'T', 'G' };

    static void Main()
    {
        string hiddenMotif;
        while (true)
        {
            Console.Write("Entrez le motif à rechercher : ");
            hiddenMotif = Console.ReadLine().Trim().ToUpper();
            
            if (string.IsNullOrEmpty(hiddenMotif))
            {
                Console.WriteLine("Erreur : Le motif ne peut pas être vide. Veuillez réessayer.");
                continue;
            }
            
            if (!hiddenMotif.All(c => Nucleotides.Contains(c)))
            {
                Console.WriteLine("Erreur : Le motif ne doit contenir que les caractères A, C, T, et G. Veuillez réessayer.");
                continue;
            }
            
            break;
        }

        int motifLength = hiddenMotif.Length;
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        int sequenceLength = 25;
        Random rand = new Random();
        int numSequences = rand.Next(20, 50);

        string[] sequences;
        int[] motifPositions;
        GenerateSequences(numSequences, sequenceLength, hiddenMotif, out sequences, out motifPositions);

        Console.WriteLine("\nSéquences ADN générées :");
        using (StreamWriter report = new StreamWriter("motif_report.txt"))
        {
            report.WriteLine("Rapport de détection de motifs\n");
            for (int i = 0; i < numSequences; i++)
            {
                string sequenceWithHighlight = HighlightMotif(sequences[i], hiddenMotif, motifPositions[i]);
                int endPosition = motifPositions[i] != -1 ? motifPositions[i] + motifLength - 1 : -1;
                Console.WriteLine($"{i + 1}: {sequenceWithHighlight} (Motif: {motifPositions[i]} - {endPosition})");
                report.WriteLine($"Séquence {i + 1}: {sequences[i]} | Début du motif: {motifPositions[i]} | Fin du motif: {endPosition}");
            }
        }

        stopwatch.Stop();
        Console.WriteLine($"\nTemps d'exécution : {stopwatch.Elapsed.TotalMilliseconds} ms");
        Console.WriteLine("\nRapport généré : motif_report.txt");
        Console.ReadKey();
    }

    static void GenerateSequences(int numSequences, int sequenceLength, string hiddenMotif, out string[] sequences, out int[] motifPositions)
    {
        Random rand = new Random();
        sequences = new string[numSequences];
        motifPositions = new int[numSequences];

        for (int i = 0; i < numSequences; i++)
        {
            bool includeMotif = rand.NextDouble() > 0.3;
            char[] sequence = new char[sequenceLength];

            for (int j = 0; j < sequenceLength; j++)
            {
                sequence[j] = Nucleotides[rand.Next(Nucleotides.Length)];
            }

            if (includeMotif)
            {
                int motifPos = rand.Next(0, sequenceLength - hiddenMotif.Length + 1);
                motifPositions[i] = motifPos;
                for (int j = 0; j < hiddenMotif.Length; j++)
                {
                    sequence[motifPos + j] = hiddenMotif[j];
                }
            }
            else
            {
                motifPositions[i] = -1;
            }

            sequences[i] = new string(sequence);
        }
    }

    static string HighlightMotif(string sequence, string motif, int motifPosition)
    {
        string highlightedSequence = "";
        if (motifPosition != -1)
        {
            for (int i = 0; i < sequence.Length; i++)
            {
                if (i >= motifPosition && i < motifPosition + motif.Length)
                    highlightedSequence += $"\u001b[32m{sequence[i]}\u001b[0m";
                else
                    highlightedSequence += sequence[i];
            }
        }
        else
        {
            int foundIndex = -1;
            foreach (var sub in motif.Select((_, i) => motif.Substring(i)).Where(sub => sequence.Contains(sub)))
            {
                foundIndex = sequence.IndexOf(sub);
                break;
            }

            for (int i = 0; i < sequence.Length; i++)
            {
                if (foundIndex != -1 && i >= foundIndex && i < foundIndex + motif.Length)
                {
                    if (i < foundIndex + motif.Length - 5)
                        highlightedSequence += $"\u001b[33m{sequence[i]}\u001b[0m";
                    else
                        highlightedSequence += $"\u001b[31m{sequence[i]}\u001b[0m";
                }
                else
                    highlightedSequence += sequence[i];
            }
        }
        return highlightedSequence;
    }
}
