namespace ShokujiMemoAndroid

open System
open ShokujiMemo.Data

[<Class>]
type MainModel() =
  let mutable pad = MemoPad.empty
