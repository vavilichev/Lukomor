# Lukomor - Unity Architectural Template

Lukomor provides Editor and script tools for easy and convenient using MVVM pattern in your projects. Lukomor was made for separating code work from editor setupping work with binders system (this work can be done by non programmers actually).
Also this framework has a lightweight DI system, it's powerful and nice addition for Lukomor.

> [!NOTE]
> If you would like to support me, please feel free to do that with [PayPal](https://paypal.me/gamedevlavka)

## Table of content:
- [Short description](https://github.com/vavilichev/Lukomor/tree/tab=readme-ov-file#short-description)
- [Installation](https://github.com/vavilichev/Lukomor/tree/tab=readme-ov-file#installation)
- [What is the MVVM](https://github.com/vavilichev/Lukomor/tree/dev?tab=readme-ov-file#what-is-the-mvvm)
- [ViewModels](https://github.com/vavilichev/Lukomor/tree/dev?tab=readme-ov-file#viewmodels)
- [Views](https://github.com/vavilichev/Lukomor/tree/dev?tab=readme-ov-file#views)
- [Binders](https://github.com/vavilichev/Lukomor/tree/dev?tab=readme-ov-file#binders)
- [What kind of binders you can expand](https://github.com/vavilichev/Lukomor/tree/dev?tab=readme-ov-file#what-kind-of-binders-you-can-expand)
- [DI in the Lukomor](https://github.com/vavilichev/Lukomor/tree/dev?tab=readme-ov-file#di-in-the-lukomor)
- [Recommendations](https://github.com/vavilichev/Lukomor/tree/dev?tab=readme-ov-file#recommendations)


## Short description

Lukomor is an architectural framework for Unity game engine that helps you apply MVVM pattern to your project easy and convenient reducing the leaks of Model into View. This framework suits to any kind of project: small and large. The most cool part of this framework (in my opinion) is separating programmers part of work from artists. Programmers can write ViewModels for features and artists can just setup binders and get a workable feature. For more information read documentation and watch [the example](https://github.com/vavilichev/LukomorExample)

## Installation
For installation, just use unity Package Manager, at the left top corner click "+" and choose "Add package from git URL..."

![image](https://github.com/vavilichev/Lukomor/assets/22970240/9fe60655-ff22-4711-88f1-dfd16bfb9c08)

Then paste the link below in the field and just press Enter.

```
https://github.com/vavilichev/Lukomor.git
```

![image](https://github.com/vavilichev/Lukomor/assets/22970240/ffe4a2de-0d92-4adc-9356-e32da9b02579)

Ready for work!

## What is the MVVM
MVVM (Model - View - ViewModel) is a simple architectural programming pattern. You can find a lot of information about it in the internet. For example in [Wikipedia](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel). Therefore, I just place a scheme of work here without any additional info.

![Lukomor Architercture-MVVM drawio (1)](https://github.com/vavilichev/Lukomor/assets/22970240/a9dc5792-9a51-4dc5-99c0-e767f99b9841)

## ViewModels
ViewModels is a non MonoBehaviour class that connects View and Model. In the Lukomor you must implement the IViewModel interface in each of your ViewModel realization. It's required because the Editor scripts works with interfaces for showing relevant information about existing ViewModels. Therefore your class can be look like this:
```csharp
public class MyCoolViewModel : IViewModel 
{

}
```

## Views
View is a basic MonoBehaviour component that must be attached to a GameObject that represents the visualization of some ViewModel`s work. It can be object on scene or prefab.

![image](https://github.com/vavilichev/Lukomor/assets/22970240/6d7bc465-f22f-4e7c-b6df-766e518220d1)

Every View can be a parent View (root) or SubView (child). The **IsParentView** checkbox in View component shows you the state. It calculates automatically.

![image](https://github.com/vavilichev/Lukomor/assets/22970240/6a398e83-a213-413c-8d51-34165a27ed61)
![image](https://github.com/vavilichev/Lukomor/assets/22970240/f885f804-618f-4c26-a48c-69859e06e80e)

Parent View and child View look simmilar but work really different. First of all, both: parent View and child View awaits binding of IViewModel.

#### Parent View
Parent View awaits ViewModel that you choose in the ViewModel property in the Editor. And then sends this ViewModel to it's child views and binders that registered in this View.

![image](https://github.com/vavilichev/Lukomor/assets/22970240/be6c1240-6721-4f75-aabb-553d0b3998bf)

#### Child View
Child View shows you other field in the Editor - PropertyName. It's because child view awaits IViewModel that contains another ViewModel inside (SubViewModel or child ViewModel). Therefore this View gets this SubViewModel from received ViewModel (directly from **property field** with the name you picked in the Editor) and do the same work: sends it to it's child Views and Binders. You also can see that all binders and child views register automatically when you add or remove them from objects, it's for optimizing runtime work.

```csharp
public class MyCoolViewModel : IViewModel
{
    public SubViewModel MyCoolSubViewModel { get; }    // must be a property with public "get" for defining by Editor scripts

    public MyCoolViewModel()
    {
        MyCoolSubViewModel = new SubViewModel();
    }
}

```

![Parent View](https://github.com/vavilichev/Lukomor/assets/22970240/556fa715-d2f3-4aca-9e03-af28b40c295c)

![Child View](https://github.com/vavilichev/Lukomor/assets/22970240/42834a0d-d684-4c14-bbd2-84fc10275c6e)

> [!TIP]
> The Search feature helps you find your ViewModels and property names really quick. 

> [!WARNING]
> When you are going to make a prefab with parent View, you have to place it outside of other View. Otherwise prefab View defines like **child View** and you cannot choose ViewModel, only property names of ViewModel.




## Binders
Binders is the main feature of this framework. Binders help you to connect View and ViewModel: visualize data from ViewModel and send signals to the ViewModel for interacting with Model for example.

```csharp
public class MyCoolViewModel : IViewModel
{
    public IObservable<string> SomeObservableText => _someObservableText;
    public IObservable<string> SomeReactivePropertyText => _someReactivePropertyText;

    private readonly Subject<string> _someObservableText = new();
    private readonly ReactiveProperty<string> _someReactivePropertyText = new();

    public MyCoolViewModel()
    {
        _someObservableText.OnNext("Your awesome observableText");
        _someReactivePropertyText.Value = "Your awesome reactivePropertyText";
    }
}

```

![image](https://github.com/vavilichev/Lukomor/assets/22970240/74e4d884-2028-4b7e-b16f-f0ad863b5a21)



There are two types of binders:

### Observable property binder (ObservableBinder<T>)

This binder subscribes on public property that implements the IObservable<T> interface. When IObservable does "next", then binder handles the changed data and changes the visual that has been setupped in the editor. 
You can create your own binders, just inherit from ObservableBinder<T> and write your type instead of T. By the way, there are a bunch of prepared binders in the Lukomor.

It's recommendet to pay attention to **UnityEventBinder<T>** type of binders. That binders use UnityEvents for handling data from ViewModel. It's very convenient for using in editor. Watch the example above. 

Also you can use implementation of **IReadonlyReactiveCollection<T>** for your binders, it's convinient for creating and destroying Views depending on collection content. For example **VMCollectionToGameObjectCreationBinder** instantiates View from picked Prefab and bind added to collection ViewModel to the created View.

```csharp
public class MyCoolViewModel : IViewModel
{
    public IReadOnlyReactiveCollection<SubViewModel> SubViewModels => _subViewModels;

    private readonly ReactiveCollection<SubViewModel> _subViewModels = new();

    public MyCoolViewModel()
    {
        var subViewModelA = new SubViewModel();
        var subViewModelB = new SubViewModel();
        
        _subViewModels.Add(subViewModelA);
        _subViewModels.Add(subViewModelB);
    }
}
```

![image](https://github.com/vavilichev/Lukomor/assets/22970240/3884728f-d153-4c12-884b-471725eb5c9d)



### MethodBinder (that named the same)
When Binder received the ViewModel, this binder grabs the method from that ViewModel with the name you picked in the editor and caches it. When you call **Perform()** method of the Binder, this binder invokes the cached method. 

```csharp
public class MyCoolViewModel : IViewModel
{
    public void EmptyMethod()
    {
        
    }

    public void FloatArgMethod(float value)
    {
        
    }
}
```

Next two variants are the same, but **ButtonMerhodBinder** is more convenient for using with Unity Buttons.

![image](https://github.com/vavilichev/Lukomor/assets/22970240/5f741c12-b369-4e60-8569-543a642d34c3)

![image](https://github.com/vavilichev/Lukomor/assets/22970240/b78251eb-a2d7-463c-bc77-95476ffd50bf)

Lukomor also suppots G**enericMethodBinder<T>** that can invoke methods with arguments. You should use Perform(T value) instead of Perform(). It's convenient for cases when player does some input actions, for example moving a handle of the Slider

![image](https://github.com/vavilichev/Lukomor/assets/22970240/afad3725-8bbd-427b-b4f6-a4d93d85aed0)

> [!IMPORTANT]
> Full list of prepared binders you can see in the **Packages/Lukomor Architecture/Lukomor/Scripts/MVVM/Binders section**.
> 
>![image](https://github.com/vavilichev/Lukomor/assets/22970240/578216e2-1a28-465f-99be-1254673bd87e)




## What kind of binders you can expand

### Binder

This is an abstract base binder that automaticaly can define ViewModel for binding and register this binder to the View when you attach binder to the Game Object. Inherit from Binder class and do what you want with ViewModel. FYI: this class has BinderEditor script for drawing ViewModel property in the Editor.
  
### ObservableBinder\<T\>

This is an abstract base ObservableBinder that has a PropertyName field. This binder automaticaly subscribes on **IObservable\<T\>** property from the received ViewModel. You can inherit form **ObservableBinder\<T\>** and define **T**, for example as a string (public class MyStringBinder : ObservableBinder\<string\>) and that binder automatically suggests only IObservable<string> property names for setupping this binder. Also it's required to write your implementation what to do with new value of the property (when it change) in the **OnPropertyChanged()** method.
  
### UnityEventBinder\<T\>

This is an abstract base UnitEventBinder (inherited from **ObservableBinder\<T\>**)  that sugests you to invoke received updated data into **UnityEvent**. It's convenient to use it with common data and transform one type of data to another. For example transform boolean value into color (isActive {true = green, false = red). FYI: BoolToColorUnityEventBinder already exists. Inherit from **UnityEventBinder\<T\>** and define your **T**, no additional actions required. But if you want something special, you can inherid from **ObservableBinder\<T\>** and make custom UnityEventBinder with desired transformations or validations.
  
### ObservableCollectionBinder\<\T>

The same as **ObservableBinder\<T\>** but works with **IReadOnlyReactiveCollection\<T\>**. And you need to implement two methods instead of one: **OnItemAdded()** and **OnItemRemoved()** in other words: what to do with the new element in the collection and what to do with removed element from collection of ViewModel (picked in the Editor)
  
### GenericMethodBinder\<T\>

This is an abstract base binder for methods with parameters binding. This binder grabs public method with **T** parameter from ViewModel and caches it. The you run **Perform(T value)** method from anywherer you want, cached method of ViewModel will invoke. Inherit from **GenericMethodBinder\<T\>** and define **T** if you want to send custom data into ViewModel.

## DI in the Lukomor
Lukomor also has a simple Dependency Injection implementation. It's optional and it's not integrated in the MVVM system. DI is just nice addition for the main framework.
Lukomor DI based on factories and has an inheritance of DI containers. Lets watch how it works in detail

### DIContainer
All dependencies (means factories) registers in DIContainer. Containers can inherit one from another with using composition, it means that you can create a tree of dependencies where child containers know about parent and can request instances from it, but parent containers doesn't know about childs. It's convenient to create one container for whole game that work like a project context, and different containers for each scene of your game. When you destroy scene, you destroy it's contaier, so all of dependencies of that scene destroyes too.

### Factories
DIContainer stores factories, therefore you have to register factories into id DIContainer. Factory is a delegate that receives a reference to a DIContainer instance (you can get another instances from this container) and returns an instance of requested type.
```csharp
Func<DIContainer, T> factory
```

#### Register like singletone
You can register factory that produces singleton (not static, of course, just instance with flag) and cache it and uses cached value every Resolve() running.

``` csharp
var container = new DIContainer();
container.RegisterSingleton(c => new MyAwesomeClass());
```

Also you can use tags system for creating unique instances of the same type for different needs:
```csharp
container.RegisterSingleton("my_owesome_tag_A", c => new MyAwesomeClass());
container.RegisterSingleton("my_owesome_tag_B", c => new MyAwesomeClass());
```

> [!IMPORTANT]
> You should know that registration doesn't creates instances instantly. Container creates instances when you request classes by calling  Resolve(). 

> [!IMPORTANT]
> You can create instance with registration by using CreateInstance() method. Each registration returns DIBuilder instance so you can do it with the registration.

```csharp
var myAwesomeClassInstance = container
   .RegisterSingleton(c => new MyAwesomeClass())
   .CreateInstance();
```

#### Register like transient
Also you can register factory for resolving multiple times for getting new instance every time when you do Resolve().
``` csharp
container.Register(c => new MyAwesomeClass());

...

var instanceA = container.Resolve<MyAwesomeClass>();
var instanceB = container.Resolve<MyAwesomeClass>();    // instanceA != instanceB
```

And transient variant has a tags approach as well.
``` csharp
container.Register("variant_1", c => new MyAwesomeClass(parameter_1));    // factory with parameter 1
container.Register("variant_2", c => new MyAwesomeClass(parameter_2));    // factory with parameter 2

...

var instanceA = container.Resolve<MyAwesomeClass>("variant_1");
var instanceB = container.Resolve<MyAwesomeClass>("variant_1");    // instanceA != instanceB

var instanceC = container.Resolve<MyAwesomeClass>("variant_2");
var instanceD = container.Resolve<MyAwesomeClass>("variant_2");    // instanceC != instanceD
```

### Inheritance
You can make project context with own parent container and scene context with own child container for making access from scene context to project context by using inheritance with simple composition:
```csharp
var projectContainer = new DIContainer();
projectContainer.RegisterSingleton<GameSettingsService>();

... 

var sceneContainer = new DIContainer(projectContainer);
sceneContainer.RegisterSingleton<CoolSceneFeatureService>();

...

var gameSettingsService = projectContainer.Resolve<GameSettingsService>();    // Work
var gameSettingsService = sceneContainer.Resolve<GameSettingsService>();    // Work

var coolSceneFeatureService = sceneContainer.Resolve<CoolSceneFeatureService>();    // Work
var coolSceneFeatureService = projectContainer.Resolve<CoolSceneFeatureService>();     // Doesn't work
```

## Recommendations
The only one recommendation: use Entry Point pattern for your projects if you use Lukomor framework. It's really convenient to separate different scopes of entities (for project, for scenes) using Lukomor DI and MVVM. Just imagine the tree of dependencies where each branch (DIContainer) can be a scope of entities (for example scene) and if you want to destroy all dependencies from this scope - destroy the branch (DIContainer).

![Lukomor Architercture-EntryPointTree drawio (2)](https://github.com/vavilichev/Lukomor/assets/22970240/e0dde927-719b-4a71-8594-aa9ddc9f4809)

