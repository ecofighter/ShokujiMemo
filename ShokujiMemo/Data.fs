module ShokujiMemo.Data

open System
open System.Linq
open System.Collections
open System.Collections.Generic
open System.Runtime.CompilerServices

[<Struct; IsReadOnly; CustomEquality; CustomComparison>]
type Date = { Year: int; Month: int; Day: int }
  with
  static member toDateTime (date:Date) : DateTime =
    new DateTime(date.Year, date.Month, date.Day)
  static member fromDateTime (datetme:DateTime) : Date =
    { Year = datetme.Year; Month = datetme.Month; Day = datetme.Day }
  member inline this.ToDateTime() =
    new DateTime(this.Year, this.Month, this.Day)
  override this.GetHashCode() =
    (this.Year, this.Month, this.Day).GetHashCode()
  override this.Equals(other:obj) : bool =
    match other with
    | :? Date as other -> (this :> IEquatable<Date>).Equals(other)
    | :? DateTime as other -> (this :> IEquatable<DateTime>).Equals(other)
    | _ -> false
  interface IEquatable<Date> with
    member this.Equals(other:Date) =
      this.Year = other.Year && this.Month = other.Month && this.Day = other.Day
  interface IEquatable<DateTime> with
    member this.Equals(other:DateTime) =
      this.ToDateTime() = other
  interface IComparable with
    member this.CompareTo(other:obj) =
      match other with
      | :? Date as other -> (this :> IComparable<Date>).CompareTo(other)
      | :? DateTime as other -> (this :> IComparable<DateTime>).CompareTo(other)
      | _ -> invalidArg "other" "Date or DateTime expected"
  interface IComparable<Date> with
    member this.CompareTo(other:Date) =
      (new DateTime(this.Year, this.Month, this.Day)).CompareTo(new DateTime(other.Year, other.Month, other.Day))
  interface IComparable<DateTime> with
    member this.CompareTo(other:DateTime) =
      (new DateTime(this.Year, this.Month, this.Day)).CompareTo(other)

[<Struct; IsReadOnly>]
type Memo = { 
  Date: Date; 
  Breakfast: string; 
  Lunch: string; 
  Dinner: string; 
  Misc: string;
  Modified: DateTime;
} with
  static member inline private convert date breakfast lunch dinner misc modified =
    { Date = date; Breakfast = breakfast; Lunch = lunch; Dinner = dinner; Misc = misc; Modified = modified }
  static member inline make date breakfast lunch dinner misc =
    Memo.convert date breakfast lunch dinner misc DateTime.Now

[<Struct>]
type MemoPad = 
  val mutable private Value: SortedList<Date, Memo>
  private new(value: SortedList<Date, Memo>) = { Value = value }
  static member empty = new MemoPad(new SortedList<Date, Memo>())
  member this.Add(memo:Memo) =
    this.Value.Add(memo.Date, memo)
  member this.Get(date:Date) =
    this.Value.[date]
  member this.Between(start:Date, finish:Date) : IEnumerable<Memo> =
    this.Value.AsReadOnly()
      .Where(fun pair -> start <= pair.Key && pair.Key <= finish)
      .Select(fun pair -> pair.Value)

  interface IEnumerable with
    member this.GetEnumerator(): IEnumerator =
      (this :> IEnumerable<Memo>).GetEnumerator()
  interface IEnumerable<Memo> with
    member this.GetEnumerator(): IEnumerator<Memo> = 
      let mutable enumerator = (this.Value.AsReadOnly() :> IEnumerable<KeyValuePair<_, Memo>>).GetEnumerator()
      { new IEnumerator<Memo> with
          member this.Current: Memo = enumerator.Current.Value
          member this.Current: obj = box<Memo> this.Current
          member this.Dispose(): unit = enumerator.Dispose()
          member this.MoveNext() = enumerator.MoveNext()
          member this.Reset() = enumerator.Reset() }
