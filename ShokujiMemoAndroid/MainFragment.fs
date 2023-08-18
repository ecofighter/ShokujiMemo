namespace ShokujiMemoAndroid

open AndroidX.Core.View
open AndroidX.Fragment.App
open AndroidX.Lifecycle

type MainMenuProvider(fragment : #Fragment) =
  inherit Java.Lang.Object()
  interface IMenuProvider with
    member this.OnCreateMenu(menu, menuInflator) =
      menuInflator.Inflate(Resource.Menu.menu_main, menu)
    member this.OnMenuItemSelected(menuItem) =
      match menuItem.ItemId with
      | Resource.Id.action_export ->
        AndroidX.Navigation.Navigation.FindNavController(fragment.View).Navigate(Resource.Id.action_to_export)
        true
      | _ -> false

type MainFragment() =
  inherit Fragment(Resource.Layout.fragment_main)

  override this.OnCreateView(inflater, container, bundle) =
    // base.OnCreateView(inflater, container, bundle)
    inflater.Inflate(Resource.Layout.fragment_main, container, false)

  override this.OnViewCreated(view, bundle) =
    base.OnViewCreated(view, bundle)

    let mutable menuHost : IMenuHost = this.RequireActivity()
    let menuProvider = new MainMenuProvider(this)
    menuHost.AddMenuProvider(menuProvider, this.ViewLifecycleOwner, Lifecycle.State.Resumed)
