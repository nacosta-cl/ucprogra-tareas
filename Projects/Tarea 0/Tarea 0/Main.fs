open BibliotecaT0
open System
open Matrixops
open Revops

let Inst = BibliotecaT0.Interfaz()
let fTablero = Inst.GetTableroFinal                 //Valores iniciales
let otablero = Inst.GetTablero
let limjugadas = Inst.GetLimiteJugadas
let mutable color = Inst.GetTurno * (-1)               // blancas = 1 negras = -1
let mutable jugadas : (int[]) list = []   //Donde se jugó. Arreglo global
let mutable listo = false       //Evaluador de salida
                         //Contador de profundidad: necesario para no pasarse con las jugadas                           //Gran funcion de resolucion

let rec Resolver intablero listo pr=                   //npm : Se pueden forzar los tipos en el input (a:int, b:string, c:int[]... etc.)
    let mutable prof = pr
    let mutable solve = listo
    let mutable tablero:int[,] = intablero
    let ctablero = tablero                  //Guardar la instancia anterior del tablero para asi poder realizar las otras posibles jugadas
    solve <- intablero=fTablero                     //Check condicion de salida
    color <- color*(-1)                                //profundidad evita sobrepasar el máximo de jugadas
    if solve = true then
        true                                           //Las jugadas estan guardadas en el array jugadas
    else                                                        //No esta listo
        let posiblesjugadas = Revops.jposibles tablero fTablero color              //Retorna un array con las posibles jugadas [|(a,b)(c,d)(e,f)|], largo = limjugadas
        let ndjp : int = List.length posiblesjugadas
        for i in 0 .. ndjp-1 do                        //Probamos cada una de nuestras opciones ya filtradas
            if prof > limjugadas then
                solve <- false
            else if prof <= limjugadas then
                prof <- prof + 1                                //todavia no alcanzamos el límite de jugadas
                let juego = posiblesjugadas.[i]
                tablero <- Revops.jugar juego.[0] juego.[1] tablero color  //Efectivamente ejecutamos la jugada en el tablero
                printfn "%A" jugadas
                Resolver tablero solve prof              //Recursion de la funcion
                if solve = true then                  
                    jugadas <- List.append jugadas [juego]                    //Guardamos la jugada en el arreglo final.
                    ()
                else                        //Si no logramos el objetivo, deshacemos la jugada
                    tablero <- ctablero
                    color <- color * (-1)
        false

Resolver otablero false 0
printfn "%A" jugadas
let Tabla = Inst.GetTablero
let Turno = Inst.GetTurno
//let pmov = Inst.PonerFicha((3,1),-1)                // Las fichas ingresan en una matriz transpuesta