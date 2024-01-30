# Lukomor - Unity Architectural Template

**Discussing here:**
https://discord.gg/yX3cKpvaGC

Table of content:
- [Short description](https://github.com/vavilichev/Lukomor/tree/dev?tab=readme-ov-file#short-description)
- [What is the MVVM](https://github.com/vavilichev/Lukomor/tree/dev?tab=readme-ov-file#what-is-the-mvvm)
- [What are the View and the Binder components](https://github.com/vavilichev/Lukomor/tree/dev?tab=readme-ov-file#what-are-the-binder-and-the-view-components)
- [How to setup View and SubView](https://github.com/vavilichev/Lukomor/tree/dev?tab=readme-ov-file#how-to-setup-view-and-subview)
- [Using Observables in the Lukomor](https://github.com/vavilichev/Lukomor/tree/dev?tab=readme-ov-file#how-to-setup-view-and-subview)
- [Basic set of binders in Lukomor](https://github.com/vavilichev/Lukomor/tree/dev?tab=readme-ov-file#basic-set-of-binders-in-lukomor)
- [How to expand the set of binders](https://github.com/vavilichev/Lukomor/tree/dev?tab=readme-ov-file#how-to-expand-the-set-of-binders)
- [How to setup binders](https://github.com/vavilichev/Lukomor/tree/dev?tab=readme-ov-file#how-to-expand-the-set-of-binders)
- [DI in the Lukomor](https://github.com/vavilichev/Lukomor/tree/dev?tab=readme-ov-file#di-in-the-lukomor)
- [Recommendations](https://github.com/vavilichev/Lukomor/tree/dev?tab=readme-ov-file#di-in-the-lukomor)


## Short description

Lukomor is an architectural framework for Unity game engine that helps you apply MVVM pattern to your project easy and convenient reducing the leaks of Model into View. This framework suits to any kind of project: small and large. The most cool part of this framework (in my opinion) is separating programmers part of work from artists. Programmers can write ViewModels for features and artists can just setup binders and get a workable feature. For more information read documentation and watch [the example](https://github.com/vavilichev/LukomorExample)

## What is the MVVM
MVVM (Model - View - ViewModel) is a simple architectural programming pattern. You can find a lot of information about it in the internet. For example in [Wikipedia](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel). Therefore, I just place a scheme of work here without any additional info.

![image](https://github.com/vavilichev/Lukomor/assets/22970240/9aef4881-09b9-4012-acc3-84b09b13db44)

## ViewModels
ViewModels is a non MonoBehaviour class that connects View and Model. In the Lukomor you must implement the IViewModel interface in each of your ViewModel realization. It's required because the Editor scripts works with interfaces for showing relevant information about existing ViewModels. Therefore your class can be look like this:
```csharp
public class MyCoolViewModel : IViewModel 
{

}
```

## View
View is a basic MonoBehaviour component that must be attached to a GameObject that represents the visualization of some ViewModel`s work. It can be object on scene or prefab.
Every View can be a SubView. It means that current View is a child of another View and it defines automatically in the Editor. Try to create a GameObject with View component and create another GameObject as a child of first GameObject and add View Component to it. You can see a isRoot checkmark  in the View component that says you the status.

### View
View component 


## How to setup View and SubView

## Using Observables in the Lukomor

## Basic set of binders in Lukomor

## How to expand the set of binders

## How to setup binders

## DI in the Lukomor

## Recommendations
