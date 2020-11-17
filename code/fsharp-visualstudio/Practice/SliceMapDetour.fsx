#r "nuget: Flips"
open Flips
open Flips.Types

let indices = [1..3]
let m1 = [for i in indices -> i, float i] |> Map.ofList
let d1 = DecisionBuilder "Test" {
             for index in indices -> Boolean
         } |> Map.ofSeq

// What we have to do to take the product of Map values
let sum1 = List.sum [for i in indices ->  m1.[i] * d1.[i]]






// What if we had a different data structure?

open Flips.SliceMap

let sm1 = SMap [for i in indices -> i, float i]
let sm2 = DecisionBuilder "Test" {
              for index in indices -> Boolean
          } |> SMap

let sm3 = sum (sm1 .* sm2)

// The Hadamard Product
// https://en.wikipedia.org/wiki/Hadamard_product_(matrices)


// SliceMaps also support slicing
let cities = ["A"; "B"; "C"]
let items = [1; 2; 3]

let itemCost = [
    "A", 1.0
    "B", 1.5
    "C", 3.0
    "D", 2.3
    "E", 3.7
]

let sm4 = SMap itemCost
sm4.["A"] // You can retrieve an item
sm4.[GreaterThan "A"] // You can slice
sm4.[Between ("B", "D")] // Subslicing

// More than 1 dimension
let itemLocationCost = [
    ("A", "Location1"), 1.0
    ("B", "Location1"), 1.5
    ("C", "Location1"), 3.0
    ("A", "Location2"), 2.0
    ("B", "Location2"), 4.5
    ("C", "Location2"), 1.0
]

let sm5 = SMap2 itemLocationCost
sm5.["A", All] // Two dimensional slicing
sm5.[All, "Location2"]
sm5.[GreaterOrEqual "B", LessThan "Location2"]

// Up to 5 dimensions