# Changelog

## [1.2.!] - 2022-06-28
Make setter for TMeshData Data in MeshGenerator class public

## [1.2.0] - 2022-06-28
Changed interface IMeshGenerator to abstract class MeshGeneratorBase for allowing Unity serialization
abstract class MeshGenerator is now a generic class MeshGenerator<TMeshData> and has a data member TMeshData Data
User defined Generator classes will define the type of 'Data'. Ex: SquareGridGenerator : MeshGenerator<GridResolution> where 
GridResolution is a struct in this case, but may be anything.
In MeshGeneratorComponent class, data member Generator is changed from type MeshGenerator to MeshGeneratorBase

## [1.1.2] - 2022-06-28
Moved Samples/MeshGeneration/SquareGridMesh/* to Under Samples/SquareGridMesh/MeshGeneration/*

## [1.1.1] - 2022-06-28
Moved Samples/SquareGridMesh/* to Under Samples/MeshGeneration/SquareGridMesh/*

## [1.1.0] - 2022-06-28
Moved mesh related members from SharedSquareGridGenerator.cs into MeshGenerationBase.cs
Renamed method CompleteJobs() to FinishMeshCalculations()
Renamed SharedSquareGridGenerator to SquareGridGenerator
Renamed MeshGeneratorBase to MeshGenerator

## [1.0.0] - 2022-06-24
This is the first release of the Mesh Generation API package.
