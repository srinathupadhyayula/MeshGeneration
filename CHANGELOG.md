# Changelog

## [1.2.7] - 2022-07-07
Fixed Sample name and path in package.json

## [1.2.6] - 2022-07-07
Renamed Samples Directory SquareGridMesh to Grid2DMeshGenerator
Renamed SquareGridGenerator class and file to Grid2DGenerator as it more accurately represents what the class does.

## [1.2.5] - 2022-07-07
In MeshGenerator.Generator() if mesh == null create new mesh i.e. mesh = new Mesh() instead of throwing an exception.

## [1.2.4] - 2022-07-07
Added boolean properties to give user options for various mesh optimizations in abstract class MeshGenerator
MeshGenerator.Generate() not calls the respective mesh optimizations based on the above properties.


## [1.2.3] - 2022-07-06
Removed abstract properties VerticesJob and TrianglesJob for abstract class MeshGeneratorBase. This is because there is no guarantee that the implementing generator class will have just one job or any jobs at all for calculating vertices and triangles
Removed the abstract implementation of the above properties from the abstract class MeshGenerator<TmeshData> for the same reason.
Removed backing member variables for those properties m_verticesJob & m_trianglesJob from the abstract class MeshGenerator<TmeshData>.
Converted properties VerticesJob and TrianglesJob to auto-properties in sample generator SquareGridGenerator class


## [1.2.2] - 2022-07-05
Fix typos and date errors in changelog.
Fixed access modifiers in abstract class MeshGeneratorBase.
Fixed access modifiers in abstract class MeshGenerator.
Moved abstract properties VertexCount and IndexCount from MeshGenerator to meshGeneratorBase
Added abstract property TriangleCount to MeshGeneratorBase.


## [1.2.1] - 2022-07-04
Make setter for TMeshData Data in MeshGenerator class public

## [1.2.0] - 2022-07-04
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
