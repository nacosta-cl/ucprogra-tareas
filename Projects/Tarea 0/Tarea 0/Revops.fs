module Revops
open Matrixops


let juegoup tabla x y ancho largo color =       //DONE
    let tablero : int [,] = tabla
    let mutable y2 : int = y
    let mutable juegoup_fret = false 
    let mutable fretsalida = false
    if y2 = 0 then
        juegoup_fret <- false                   //Nada arriba
    else
        while y2>0 && juegoup_fret = false && fretsalida = false do
            if (tablero.[x,(y2-1)]=0) || (y2-1 = 0) || tablero.[x,(y2-1)] = (color)then
                fretsalida <- true
            else if (tablero.[x,(y2-2)] = color) && (tablero.[x,(y2-1)] = (color*(-1))) then         //Se cumple el patron hacia abajo
                juegoup_fret <- true
            y2 <- y2 - 1
    juegoup_fret

let juegodown tabla x y ancho largo color =     //DONE
    let tablero : int [,] = tabla
    let mutable y2 : int = y
    let mutable juegodown_fret = false 
    let mutable fretsalida = false
    if y2 = largo-1 then
        juegodown_fret <- false                   //Nada abajo
    else
        while y2<largo-1 && juegodown_fret = false && fretsalida = false do
            if (tablero.[x,(y2+1)]=0) || (y2+1 = largo-1) ||  (tablero.[x,(y2+1)] = (color)) then
                fretsalida <- true
            else if (tablero.[x,(y2+2)] = color) && (tablero.[x,(y2+1)] = (color*(-1))) then         //Se cumple el patron hacia abajo
                juegodown_fret <- true
            y2 <- y2 + 1
    juegodown_fret

let juegoleft tabla x y ancho largo color =       //DONE
    let tablero : int [,] = tabla
    let mutable x2 : int = x
    let mutable juegoleft_fret = false 
    let mutable fretsalida = false
    if x2 = 0 then
        juegoleft_fret <- false                   //Nada arriba
    else
        while x2>0 && juegoleft_fret = false && fretsalida = false do
            if (tablero.[(x2-1),y] = 0) || (x2-1 = 0) || (tablero.[(x2-1),y] = color) then
                fretsalida <- true
            else if (tablero.[(x2-2),y] = color) && (tablero.[(x2-1),y] = (color*(-1))) then         //Se cumple el patron hacia abajo
                juegoleft_fret <- true
            x2 <- x2 - 1
    juegoleft_fret

let juegoright tabla x y ancho largo color =     //DONE
    let tablero : int [,] = tabla
    let mutable x2 : int = x
    let mutable juegoright_fret = false 
    let mutable fretsalida = false
    if x2 = ancho-1 then
        juegoright_fret <- false                   //Nada abajo
    else
        while x2<ancho-1 && juegoright_fret = false && fretsalida = false do
            if (tablero.[(x2+1),y]=0) || (x2+1 = ancho-1) ||  (tablero.[(x2+1),y] = color) then
                fretsalida <- true
            else if (tablero.[(x2+2),y] = color) && (tablero.[(x2+1),y] = (color*(-1))) then         //Se cumple el patron hacia abajo
                juegoright_fret <- true
            x2 <- x2 + 1
    juegoright_fret

let juegodul tabla x y ancho largo color =     //DONE
    let tablero : int [,] = tabla
    let mutable x2 : int = x
    let mutable y2 : int = y
    let mutable juegodul_fret = false 
    let mutable fretsalida = false
    if x2 = 0 || y2 = 0 then
        juegodul_fret <- false                   //Nada abajo
    else
        while x2>0 && y2>0 && juegodul_fret = false && fretsalida = false do
            if (tablero.[(x2-1),(y2-1)]=0) || y2-1=0 || x2-1=0 || (tablero.[x2-1,y2-1] = (color)) then
                fretsalida <- true
            else if (tablero.[x2-2,y2-2] = color) && (tablero.[x2-1,y2-1] = (color*(-1))) then         //Se cumple el patron hacia abajo
                juegodul_fret <- true
            x2 <- x2 - 1
            y2 <- y2 - 1
    juegodul_fret

let juegodur tabla x y ancho largo color =     //DONE
    let tablero : int [,] = tabla
    let mutable x2 : int = x
    let mutable y2 : int = y
    let mutable juegodur_fret = false 
    let mutable fretsalida = false
    if x2 = ancho-1 || y2 = 0 then
        juegodur_fret <- false                   //Nada abajo
    else
        while x2<ancho-1 && y2>0 && juegodur_fret = false && fretsalida = false do
            if (tablero.[(x2+1),(y2-1)]=0) || y2-1=0 || x2+1=ancho-1 || (tablero.[x2+1,y2-1] = (color)) then
                fretsalida <- true
            else if (tablero.[x2+2,y2-2] = color) && (tablero.[x2+1,y2-1] = (color*(-1))) then         //Se cumple el patron hacia abajo
                juegodur_fret <- true
            x2 <- x2 + 1
            y2 <- y2 - 1
    juegodur_fret

let juegoddl tabla x y ancho largo color =     //DONE
    let tablero : int [,] = tabla
    let mutable x2 : int = x
    let mutable y2 : int = y
    let mutable juegoddl_fret = false 
    let mutable fretsalida = false
    if x2 = 0 || y2 = largo-1 then
        juegoddl_fret <- false                   //Nada abajo
    else
        while x2>0 && y2<largo-1 && juegoddl_fret = false && fretsalida = false do
            if (tablero.[(x2-1),(y2+1)]=0) || y2+1=largo-1 || x2-1=0 || (tablero.[x2-1,y2+1] = (color)) then
                fretsalida <- true
            else if (tablero.[x2-2,y2+2] = color) && (tablero.[x2-1,y2+1] = (color*(-1))) then         //Se cumple el patron hacia abajo
                juegoddl_fret <- true
            x2 <- x2 - 1
            y2 <- y2 + 1
    juegoddl_fret

let juegoddr tabla x y ancho largo color =     //DONE
    let tablero : int [,] = tabla
    let mutable x2 : int = x
    let mutable y2 : int = y
    let mutable juegoddr_fret = false 
    let mutable fretsalida = false
    if x2 = ancho-1 || y2 = largo-1 then
        juegoddr_fret <- false                   //Nada abajo
    else
        while x2<ancho-1 && y2<largo-1 && juegoddr_fret = false && fretsalida = false do
            if (tablero.[(x2+1),(y2+1)]=0) || y2+1=largo-1 || x2+1=ancho-1 || (tablero.[x2+1,y2+1] = (color)) then
                fretsalida <- true
            else if (tablero.[x2+2,y2+2] = color) && (tablero.[x2+1,y2+1] = (color*(-1))) then         //Se cumple el patron hacia abajo
                juegoddr_fret <- true
            x2 <- x2 + 1
            y2 <- y2 + 1
    juegoddr_fret

let jposibles tablero ftablero color=    //Numero de jugadas para el array , tablero original, tablero final, color a jugar...
    let tablerodj : int[,] = dmatrix tablero ftablero                  //tablero con todas las jugada hechas
    let ancho = Array2D.length1 tablero          //x
    let largo = Array2D.length2 tablero          //y
    let mutable Jugadas : int[] list = []
    let mutable temp = Array.zeroCreate<int> 2
    for x in 0..ancho-1 do
        for y in 0..largo-1 do
            if tablerodj.[x,y] = 1 then
                if juegoup tablero x y ancho largo color = true || juegodown tablero x y ancho largo color = true || juegoleft tablero x y ancho largo color = true || juegoright tablero x y ancho largo color = true || juegodul tablero x y ancho largo color = true || juegodur tablero x y ancho largo color = true || juegoddl tablero x y ancho largo color = true || juegoddr tablero x y ancho largo color = true then
                    temp.[0]<- x
                    temp.[1]<- y
                    Jugadas <- temp :: Jugadas
    Jugadas

let poneficha tabla x y xp yp color =
    let mutable x2 = x
    let mutable y2 = y
    let mutable tablero : int[,] = tabla
    let mutable out = false
    tablero.[x,y] <- color
    while out = false do
        if (tablero.[(x+xp),(y+xp)] = color*(-1)) then
            tablero.[(x+xp),(y+xp)] <- color
        else         //Se cumple el patron hacia abajo
            out <- true
        x2 <- x2 + xp
        y2 <- y2 + yp
    tablero
        
let jugar x y tablero color= 
    let mutable tabla : int [,] = tablero
    let ancho = Array2D.length1 tablero          //x
    let largo = Array2D.length2 tablero          //y
    let a = true 
    match a with
        |a when a = juegoup tablero x y ancho largo color -> tabla <- poneficha tabla x y 0 -1 color
        |a when a = juegodown tablero x y ancho largo color-> tabla <- poneficha tabla x y 0 1 color
        |a when a = juegoleft tablero x y ancho largo color -> tabla <- poneficha tabla x y -1 0 color
        |a when a = juegoright tablero x y ancho largo color-> tabla <- poneficha tabla x y 1 0  color
        |a when a = juegodul tablero x y ancho largo color-> tabla <- poneficha tabla x y -1 1 color
        |a when a = juegodur tablero x y ancho largo color-> tabla <- poneficha tabla x y 1 1 color
        |a when a = juegoddl tablero x y ancho largo color-> tabla <- poneficha tabla x y -1 -1 color
        |a when a = juegoddr tablero x y ancho largo color-> tabla <- poneficha tabla x y 1 -1 color
        |_ -> ()
    let fret = tabla
    fret