namespace ShokujiMemoAndroid

open System

open Android.App
open Android.Content
open Android.OS
open Android.Runtime
open Android.Views
open Android.Util
open Android.Widget

open ShokujiMemo.Data

[<Activity (Label = "@string/app_name", MainLauncher = true, Icon = "@mipmap/icon")>]
type MainActivity () =
  inherit Activity ()

  do Resource.UpdateIdValues()

  let mutable selectedDate : Date = Date.fromDateTime(DateTime.Now)

  override this.OnCreate (bundle) =
    base.OnCreate (bundle)

    // Set our view from the "main" layout resource
    this.SetContentView(Resource.Layout.Main)

    let calendar = this.FindViewById<CalendarView>(Resource.Id.calendarView1)
    // let since = new DateTime(1970, 1, 1, 0, 0, 0)
    // let duration = selectedDate.ToDateTime() - since
    // calendar.SetDate((int64)duration.TotalMilliseconds, true, true)

    let toolbar = this.FindViewById<Toolbar>(Resource.Id.toolbar);
    toolbar.SetTitle(Resource.String.app_name)
    this.SetActionBar(toolbar)

    let button = this.FindViewById<Button>(Resource.Id.button1)
    button.Click.Add (fun _args  ->
      selectedDate <- Date.fromDateTime(DateTime.Now)
      let since = new DateTime(1970, 1, 1, 0, 0, 0)
      let duration = selectedDate.ToDateTime() - since
      calendar.SetDate((int64)duration.TotalMilliseconds, true, true)
      Log.Debug("ShokujiMemoAndroid", "Button Clicked") |> ignore
    )

  override this.OnStop() =
    Log.Debug("ShokujiMemoAndroid", "OnStop") |> ignore
    base.OnStop ()

  override this.OnSaveInstanceState (outState) =
    outState.PutInt("selectedDateYear", selectedDate.Year)
    outState.PutInt("selectedDateMonth", selectedDate.Month)
    outState.PutInt("selectedDateDay", selectedDate.Day)
    Log.Debug("ShokujiMemoAndroid", "Saved InstanceState") |> ignore
    base.OnSaveInstanceState (outState)

  override this.OnRestoreInstanceState (savedInstanceState) =
    selectedDate <-
      try
        let (year, month, day) = (savedInstanceState.GetInt("selectedDateYear"), savedInstanceState.GetInt("selectedDateMonth"), savedInstanceState.GetInt("selectedDateDay"))
        Log.Debug("ShokujiMemoAndroid", sprintf "%d %d %d" year month day) |> ignore
        { Year = year; Month = month; Day = day }
      with
        _ -> Log.Debug("ShokujiMemoAndroid", "Not Found Date in Bundle") |> ignore
             Date.fromDateTime(DateTime.Now)

    let calendar = this.FindViewById<CalendarView>(Resource.Id.calendarView1)
    let since = new DateTime(1970, 1, 1, 0, 0, 0)
    let duration = selectedDate.ToDateTime() - since
    calendar.SetDate((int64)duration.TotalMilliseconds, true, true)
