# Identification de Motifs

Projet réalisé pour la matière **IA FIN** en Master 1 de Finance à l'ECE Paris.

## Présentation
Ce projet consiste à identifier des motifs ADN dans des séquences générées aléatoirement. L'objectif est d'explorer les capacités d'**Infer.NET**, une bibliothèque de Microsoft dédiée à l'inférence probabiliste, et d'appliquer des techniques de reconnaissance de motifs dans des données biologiques synthétiques.

## Prérequis
Avant de reproduire cette expérience, il est recommandé de :
1. **Consulter les tutoriels de la bibliothèque Infer.NET** : Cela permet de comprendre le fonctionnement des modèles probabilistes et leur implémentation en C#.
   - [Documentation officielle](https://dotnet.github.io/infer/)
   - Exemples et guides disponibles sur GitHub
2. **Expérimenter avec les codes fournis dans les tutoriels** pour mieux comprendre l'utilisation des distributions et des inférences probabilistes.
3. **Utiliser une assistance IA** comme ChatGPT pour résoudre des problèmes spécifiques et améliorer la compréhension des concepts.

## Étapes pour reproduire l'expérience

### 1. Installation des dépendances
Assurez-vous d'avoir installé **.NET SDK** et les bibliothèques nécessaires :
```sh
# Installer Infer.NET
nuget install Microsoft.ML.Probabilistic
```
Ou via le fichier `.csproj` :
```xml
<ItemGroup>
    <PackageReference Include="Microsoft.ML.Probabilistic" Version="*.**.*" />
</ItemGroup>
```

### 2. Configuration du projet
- Créez un projet C# sous **Visual Studio** ou via la ligne de commande :
  ```sh
  dotnet new console -n MotifFinder
  cd MotifFinder
  ```
- Ajoutez le fichier **MotifFinder.cs** et collez le code source du projet.

### 3. Exécution du programme
Compilez et exécutez le programme avec :
```sh
dotnet run
```
Suivez les instructions pour entrer un motif et observer la génération des séquences ADN.

### 4. Analyse des résultats
- Vérifiez les sorties dans la console, notamment la mise en surbrillance des motifs trouvés.
- Un rapport est généré sous le nom **motif_report.txt** contenant les séquences et leurs positions.

## Améliorations possibles
- Intégration d'algorithmes plus avancés pour la recherche de motifs (Boyer-Moore, KMP, distance de Levenshtein).
- Optimisation des performances avec le **multi-threading**.
- Ajout d'une interface graphique pour une meilleure visualisation.

## Conclusion
Ce projet illustre l'application de l'inférence probabiliste dans un cadre de reconnaissance de motifs ADN. Une meilleure compréhension d'Infer.NET, couplée à l'utilisation d'assistants IA comme ChatGPT, permet d'explorer efficacement ce domaine.
