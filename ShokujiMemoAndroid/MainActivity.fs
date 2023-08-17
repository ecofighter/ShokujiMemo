namespace ShokujiMemoAndroid

open System

open Android.App
open Android.Util
open Android.Widget

open AndroidX.Core.App
open AndroidX.AppCompat.App
open AndroidX.Lifecycle
open AndroidX.Fragment.App

open ShokujiMemo.Data

[<Activity (Label = "@string/app_name", MainLauncher = true, Icon = "@mipmap/icon")>]
type MainActivity () =
  inherit AppCompatActivity (Resource.Layout.activity_main)

  do Resource.UpdateIdValues()

  [<DefaultValue>] val mutable ViewModel: MainViewModel

  override this.OnCreate (bundle) =
    base.OnCreate (bundle)
    this.SetContentView (Resource.Layout.activity_main)
    if bundle = null then
      this.SupportFragmentManager.BeginTransaction()
        .SetReorderingAllowed(true)
        .Add(Resource.Id.fragment_container_view, new MainFragment())
        .Commit() |> ignore

    this.ViewModel <- (new ViewModelProvider(this)).Get(Java.Lang.Class.FromType(typeof<MainViewModel>)) :?> MainViewModel

    // let calendar = this.FindViewById<CalendarView>(Resource.Id.calendarView1)
    // let since = new DateTime(1970, 1, 1, 0, 0, 0)
    // let duration = selectedDate.ToDateTime() - since
    // calendar.SetDate((int64)duration.TotalMilliseconds, true, true)

    // let toolbar = this.FindViewById<Toolbar>(Resource.Id.toolbar);
    // toolbar.SetTitle(Resource.String.app_name)
    // this.SetActionBar(toolbar)

    // let button = this.FindViewById<Button>(Resource.Id.button1)
    // button.Click.Add (fun _args  ->
    //   let selected = Date.fromDateTime(DateTime.Now)
    //   this.ViewModel.SelectDate(selected)
    //   let since = new DateTime(1970, 1, 1, 0, 0, 0)
    //   let duration = selected.ToDateTime() - since
    //   calendar.SetDate((int64)duration.TotalMilliseconds, true, true)
    //   Log.Debug("ShokujiMemoAndroid", "Button Clicked") |> ignore
    // )

    // if bundle <> null then
    //   let selectedDate =
    //     try
    //       let (year, month, day) = (bundle.GetInt("selectedDateYear"), bundle.GetInt("selectedDateMonth"), bundle.GetInt("selectedDateDay"))
    //       Log.Debug("ShokujiMemoAndroid", sprintf "Restored: %d %d %d" year month day) |> ignore
    //       { Year = year; Month = month; Day = day }
    //     with
    //       _ -> Log.Debug("ShokujiMemoAndroid", "Not Found Date in Bundle") |> ignore
    //            Date.fromDateTime(DateTime.Now)
    //   this.ViewModel.SelectDate(selectedDate)
    //   let since = new DateTime(1970, 1, 1, 0, 0, 0)
    //   let duration = selectedDate.ToDateTime() - since
    //   calendar.SetDate((int64)duration.TotalMilliseconds, true, true)


  override this.OnStop() =
    Log.Debug("ShokujiMemoAndroid", "OnStop") |> ignore
    base.OnStop ()

  override this.OnSaveInstanceState (outState) =
    let selectedDate = (this.ViewModel.UiState.Value :?> UiState).Date
    outState.PutInt("selectedDateYear", selectedDate.Year)
    outState.PutInt("selectedDateMonth", selectedDate.Month)
    outState.PutInt("selectedDateDay", selectedDate.Day)
    Log.Debug("ShokujiMemoAndroid", "Saved InstanceState") |> ignore
    base.OnSaveInstanceState (outState)
