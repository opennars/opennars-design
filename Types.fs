module Types

open System
open System.Threading

type Truth = {F : float32; C : float32}
type Budget = {P : float32; D : float32}

type Tense = | Past | Present | Future | TenseInterval of int64 // Used by Parser

type Relop = | Inh | Sim | Imp | Equ | ExtSet | IntSet | ExtInt | IntInt | ExtDif | IntDif 
             | Prod | ExtImg | IntImg 
             | Not | And | Or 
             | QVar | DVar | IVar 
             | PreImp | ConImp | RetImp | ConEqu | PreEqu 
             | Par | Seq 
             | Operator

type OpCode =
    | Inh                                           // NAL 1
    | Sim       | ExtSet    | IntSet                // NAL 2
    | ExtInt    | IntInt    | ExtDif     | IntDif   // NAL 3
    | Prod      | ExtImg    | IntImg                // NAL 4
    | Not       | And       | Or                    // NAL 5
    | Imp       | Equ                               // NAL 5
    | PreImp    | ConImp    | RetImp                // NAL 7
    | ConEqu    | PreEqu                            // NAL 7
    | Par       | Seq                               // NAL 7
    | Operator                                      // NAL 8

type VarCode = | QVar | DVar | IVar
type Term = | Word of string | Interval of int32 | Var of VarCode * string | Term of OpCode * Term list
type TaskType = | Belief | Goal | Question | Quest

type Id = int64
type Occurrence = | Event of int64 | Eternal | NoTense  // NoTense used by tasks that have no tense i.e. goal and quest

type Sentence = { TaskType : TaskType; Term : Term; Occurrence : Occurrence; Truth : Truth option }

type ParsedTask = ParsedTask of Budget * Sentence

type Stamp = {ID : int64; Evidence : Id list; Created : int64; SC : int}

[<CustomComparison>]
[<StructuralEquality>]
type Task = 
    { Budget : Budget; Sentence : Sentence; Terms : Term list; Stamp : Stamp }
    interface System.IComparable<Task> with
        member this.CompareTo other =
            other.Budget.P.CompareTo(this.Budget.P)

[<CustomComparison>]
[<StructuralEquality>]
type TEPair = 
    {Truth : Truth; Evidence : int64 list}
    interface System.IComparable<TEPair> with
        member this.CompareTo other =
            other.Truth.C.CompareTo(this.Truth.C)

[<CustomComparison>]
[<StructuralEquality>]
type TaskBelief = 
    { Task: Task; Belief: Task; P: float32}
    interface System.IComparable<TaskBelief> with
        member this.CompareTo other =
            other.P.CompareTo(this.P)

type BestAnswer = {Term: Term; TEPair: TEPair; ID : int64}

let startTime = DateTime.Now.Ticks
let systemTime() = startTime / 10000L

let Id = ref 0L
let ID() = Interlocked.Increment(Id)