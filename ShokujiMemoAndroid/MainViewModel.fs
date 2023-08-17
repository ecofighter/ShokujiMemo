namespace ShokujiMemoAndroid

open System
open ShokujiMemo.Data
open AndroidX.Lifecycle

type UiState(date, breakfast, lunch, dinner, misc) =
  inherit Java.Lang.Object()
  member val Date: Date = date with get, set
  member val Breakfast: string = breakfast with get
  member val Lunch: string = lunch with get
  member val Dinner: string = dinner with get
  member val Misc: string = misc with get
  new() = new UiState(Date.fromDateTime(DateTime.Now), "", "", "", "")

type MainViewModel() =
  inherit ViewModel()
  member val UiState: MutableLiveData = new MutableLiveData(new UiState()) with get
  member this.SelectDate(date:Date) : unit =
    let oldState = this.UiState.Value :?> UiState
    this.UiState.SetValue(new UiState(date, oldState.Breakfast, oldState.Lunch, oldState.Dinner, oldState.Misc))
