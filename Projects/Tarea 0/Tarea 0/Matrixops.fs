module Matrixops
open System.Numerics
let absval n:int=
    match n with
    |n when n<0 -> n*(-1)
    |n when n>0 -> n
    |_ -> 0
 
let transpone (matrix : _[,]) = 
    let ancho = Array2D.length1 matrix          //x
    let largo = Array2D.length2 matrix          //y
    let mutable Nmatrix = Array2D.zeroCreate ancho largo               //Ancho:x - Largo:y
    for y in 0..largo-1 do
        for x in 0..ancho-1 do
            Nmatrix.[y,x] <- matrix.[x,y]
    Nmatrix

let dmatrix (matrixor : int[,]) (matrixdesc : int[,]) =             //Obtiene la diferencia entre 2 matrices
    let mutable pardata = Array.zeroCreate<int> 2
    let ancho = Array2D.length1 matrixor          //x
    let largo = Array2D.length2 matrixor           //y
    let mutable Nmatrix = Array2D.zeroCreate<int> ancho largo               //Ancho:x - Largo:y 
    for y in 0..largo-1 do
        for x in 0..ancho-1 do
            let a : int = matrixor.[x,y]
            let b : int = matrixdesc.[x,y]
            match (a,b) with
            |(0,-1) | (0,1) -> Nmatrix.[x,y] <- 1
            |_ -> ()
    Nmatrix       

let producto_matricial =
    "We'll see"
