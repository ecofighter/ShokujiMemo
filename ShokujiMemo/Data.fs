module ShokujiMemo.Data

open System
open System.Runtime.CompilerServices

[<Struct; IsReadOnly; RequireQualifiedAccess>]
type Memo = { Date: DateTime; Contents: string; Modified: DateTime; } with
  static member make date contents =
    { Date = date; Contents = contents; Modified = DateTime.Now; }
  static member convert date contents modified =
    { Date = date; Contents = contents; Modified = modified; }

[<Struct; IsReadOnly; RequireQualifiedAccess>]
type MemoPad =
  val private Value: Map<DateTime, (string * DateTime)>
  private new(value) = { Value = value; }
  member this.Add(date:DateTime, contents:string, modified:DateTime) =
    this.Value.Add(date, (contents, modified))
  member this.Add(memo:Memo) =
    this.Value.Add(memo.Date, (memo.Contents, memo.Modified))
  member this.Between(start:DateTime, finish:DateTime) : MemoPad =
    let filtered = Map.filter (fun k _ -> start <= k && k <= finish) this.Value
    in new MemoPad(filtered)
  static member Empty = new MemoPad(Map.empty)
