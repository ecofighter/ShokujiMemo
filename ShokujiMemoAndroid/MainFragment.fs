namespace ShokujiMemoAndroid

open AndroidX.Fragment.App

type MainFragment() =
  inherit Fragment(Resource.Layout.fragment_main)

  override this.OnCreate(bundle) =
    base.OnCreate(bundle)
    this.HasOptionsMenu <- false
