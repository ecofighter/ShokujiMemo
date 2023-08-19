namespace ShokujiMemoAndroid

open Android.Views
open AndroidX.AppCompat.Widget
open AndroidX.Core.View
open AndroidX.Fragment.App
open AndroidX.Lifecycle
open AndroidX.Navigation

type ExportMenuProvider(fragment: Fragment) =
  inherit Java.Lang.Object()
  interface IMenuProvider with
    member this.OnCreateMenu(menu, menuInflator) =
      menuInflator.Inflate(Resource.Menu.menu_export, menu)
    member this.OnMenuItemSelected(menuItem) =
      match menuItem.ItemId with
      | Resource.Id.action_export ->
        Navigation.FindNavController(fragment.View).Navigate(Resource.Id.action_to_export)
        true
      | _ -> false

type ExportButtonBackOnClickListener (fragment: Fragment) =
  inherit Java.Lang.Object()
  interface View.IOnClickListener with
    member this.OnClick(view) =
      let res = Navigation.FindNavController(fragment.View).PopBackStack()
      if res then () else failwith "Failed to pop back stack"

type ExportFragment() =
  inherit Fragment(Resource.Layout.fragment_export)

  override this.OnCreateView(inflater, container, bundle) =
    inflater.Inflate(Resource.Layout.fragment_export, container, false)

  override this.OnViewCreated(view, bundle) =
    base.OnViewCreated(view, bundle)

    let mutable menuHost: IMenuHost = this.RequireActivity()
    let menuProvider = new ExportMenuProvider(this)
    menuHost.AddMenuProvider(menuProvider, this.ViewLifecycleOwner, Lifecycle.State.Resumed)
    
    let button1 = this.View.FindViewById<AppCompatButton>(Resource.Id.export_button_back)
    button1.SetOnClickListener(new ExportButtonBackOnClickListener(this))
