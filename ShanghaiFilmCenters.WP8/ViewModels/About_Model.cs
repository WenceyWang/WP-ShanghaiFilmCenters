using MVVMSidekick.Reactive;
using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShanghaiFilmCenters.WP8.ViewModels
{
    public class About_Model : ViewModelBase<About_Model>
    {

        //public CommandModel<ReactiveCommand, String> CommandGoToHelpPage
        //{
        //    get { return _CommandGoToHelpPageLocator(this).Value; }
        //    set { _CommandGoToHelpPageLocator(this).SetValueAndTryNotify(value); }
        //}
        //#region Property CommandModel<ReactiveCommand, String> CommandGoToHelpPage Setup
        //protected Property<CommandModel<ReactiveCommand, String>> _CommandGoToHelpPage = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandGoToHelpPageLocator };
        //static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandGoToHelpPageLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandGoToHelpPage", model => model.Initialize("CommandGoToHelpPage", ref model._CommandGoToHelpPage, ref _CommandGoToHelpPageLocator, _CommandGoToHelpPageDefaultValueFactory));
        //static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandGoToHelpPageDefaultValueFactory =
        //    model =>
        //    {
        //        var resource = "GoToHelpPage";           // Command resource  
        //        var commandId = "GoToHelpPage";
        //        var vm = CastToCurrentType(model);
        //        var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
        //        cmd
        //            .DoExecuteUIBusyTask(
        //                vm,
        //                async e =>
        //                {
        //                    await vm.StageManager.DefaultStage.Show(new Help_Model());
        //                }
        //            )
        //            .DoNotifyDefaultEventRouter(vm, commandId)
        //            .Subscribe()
        //            .DisposeWith(vm);
        //        return cmd.CreateCommandModel(resource);
        //    };
        //#endregion


        //public CommandModel<ReactiveCommand, String> CommandGoToRating
        //{
        //    get { return _CommandGoToRatingLocator(this).Value; }
        //    set { _CommandGoToRatingLocator(this).SetValueAndTryNotify(value); }
        //}
        //#region Property CommandModel<ReactiveCommand, String> CommandGoToRating Setup
        //protected Property<CommandModel<ReactiveCommand, String>> _CommandGoToRating = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandGoToRatingLocator };
        //static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandGoToRatingLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandGoToRating", model => model.Initialize("CommandGoToRating", ref model._CommandGoToRating, ref _CommandGoToRatingLocator, _CommandGoToRatingDefaultValueFactory));
        //static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandGoToRatingDefaultValueFactory =
        //    model =>
        //    {
        //        var resource = "GoToRating";           // Command resource  
        //        var commandId = "GoToRating";
        //        var vm = CastToCurrentType(model);
        //        var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
        //        cmd
        //            .DoExecuteUIBusyTask(
        //                vm,
        //                async e =>
        //                {
        //                    //Todo: Add GoToRating logic here, or
        //                    await MVVMSidekick.Utilities.TaskExHelper.Yield();
        //                }
        //            )
        //            .DoNotifyDefaultEventRouter(vm, commandId)
        //            .Subscribe()
        //            .DisposeWith(vm);
        //        return cmd.CreateCommandModel(resource);
        //    };
        //#endregion

    }
}
