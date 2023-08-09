namespace ShokujiMemoAndroid

open System

open Android.App
open Android.Content
open Android.OS
open Android.Runtime
open Android.Views
open Android.Widget

[<Activity (Label = "ShokujiMemoAndroid", MainLauncher = true, Icon = "@mipmap/icon")>]
type MainActivity () =
    inherit Activity ()

    do Resource.UpdateIdValues()

    let mutable count:int = 1

    override this.OnCreate (bundle) =
        base.OnCreate (bundle)

        // Set our view from the "main" layout resource
        this.SetContentView(Resource.Layout.Main)

        let calendar = this.FindViewById<CalendarView>(Resource.Id.calendarView1)
        let currently = DateTime.Now
        let since = new DateTime(1970, 1, 1, 0, 0, 0)
        let duration = currently - since
        // Set CalendarView to current date
        calendar.SetDate((int64)duration.TotalMilliseconds, true, false)
        let toolbar = this.FindViewById<Toolbar>(Resource.Id.toolbar);
        toolbar.SetTitle(Resource.String.app_name)
        this.SetActionBar(toolbar)

        let button = this.FindViewById<Button>(Resource.Id.button1)
        button.Click.Add (fun _args  ->
          let currently = DateTime.Now
          let since = new DateTime(1970, 1, 1, 0, 0, 0)
          let duration = currently - since
          calendar.SetDate((int64)duration.TotalMilliseconds, true, true)
        )
