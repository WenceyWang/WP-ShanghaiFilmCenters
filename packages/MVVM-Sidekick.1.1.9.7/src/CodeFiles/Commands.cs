﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Commands;
using System.Runtime.CompilerServices;
using MVVMSidekick.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Collections;
using MVVMSidekick.Utilities;
using MVVMSidekick.Patterns;
using MVVMSidekick.Collections;
using MVVMSidekick.Views;
using MVVMSidekick.EventRouting;
#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
using System.Collections.Concurrent;
using Windows.UI.Xaml.Navigation;

using Windows.UI.Xaml.Controls.Primitives;

#elif WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Concurrent;
using System.Windows.Navigation;

using System.Windows.Controls.Primitives;

#elif SILVERLIGHT_5||SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#elif WINDOWS_PHONE_8||WINDOWS_PHONE_7
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#endif





namespace MVVMSidekick
{

    namespace Commands
    {
        /// <summary>
        /// Command被运行触发的事件数据类型
        /// </summary>
        public class EventCommandEventArgs : EventArgs
        {
            public Object Parameter { get; set; }
            public Object ViewModel { get; set; }
            public Object ViewSender { get; set; }
            public Object EventArgs { get; set; }
            public string EventName { get; set; }
            public Type EventHandlerType { get; set; }
            public static EventCommandEventArgs Create(
                Object parameter = null,
                Object viewModel = null,
                object viewSender = null,
                object eventArgs = null,
               string eventName = null,
                Type eventHandlerType = null
                )
            {
                return new EventCommandEventArgs { Parameter = parameter, ViewModel = viewModel, ViewSender = viewSender, EventArgs = eventArgs, EventHandlerType = eventHandlerType, EventName = eventName };
            }
        }

        /// <summary>
        /// 事件Command的助手类
        /// </summary>
        public static class EventCommandHelper
        {
            /// <summary>
            /// 为一个事件Command制定一个VM
            /// </summary>
            /// <typeparam name="TCommand">事件Command具体类型</typeparam>
            /// <param name="cmd">事件Command实例</param>
            /// <param name="viewModel">VM实例</param>
            /// <returns>事件Command实例本身</returns>
            public static TCommand WithViewModel<TCommand>(this TCommand cmd, BindableBase viewModel)
                where TCommand : EventCommandBase
            {
                cmd.ViewModel = viewModel;
                return cmd;
            }

        }

        /// <summary>
        /// 带有VM的Command接口
        /// </summary>
        public interface ICommandWithViewModel : ICommand
        {
            BindableBase ViewModel { get; set; }
        }

        /// <summary>
        /// 事件Command,运行后马上触发一个事件，事件中带有Command实例和VM实例属性
        /// </summary>
        public abstract class EventCommandBase : ICommandWithViewModel
        {
            /// <summary>
            /// VM
            /// </summary>
            public BindableBase ViewModel { get; set; }

            /// <summary>
            /// 运行时触发的事件
            /// </summary>
            public event EventHandler<EventCommandEventArgs> CommandExecute;
            /// <summary>
            /// 执行时的逻辑
            /// </summary>
            /// <param name="args">执行时的事件数据</param>
            internal protected virtual void OnCommandExecute(EventCommandEventArgs args)
            {
                if (CommandExecute != null)
                {
                    CommandExecute(this, args);
                }
            }


            /// <summary>
            /// 该Command是否能执行
            /// </summary>
            /// <param name="parameter">判断参数</param>
            /// <returns>是否</returns>
            public abstract bool CanExecute(object parameter);

            /// <summary>
            /// 是否能执行的值产生变化的事件
            /// </summary>
            public event EventHandler CanExecuteChanged;

            /// <summary>
            /// 是否能执行变化时触发事件的逻辑
            /// </summary>
            protected void OnCanExecuteChanged()
            {
                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, EventArgs.Empty);
                }
            }

            /// <summary>
            /// 执行Command
            /// </summary>
            /// <param name="parameter">参数条件</param>
            public virtual void Execute(object parameter)
            {
                if (CanExecute(parameter))
                {
                    OnCommandExecute(EventCommandEventArgs.Create(parameter, ViewModel));
                }
            }



        }



        namespace EventBinding
        {






            public class CommandBinding : FrameworkElement

            {

                public CommandBinding()
                {
                    base.Width = 0;
                    base.Height = 0;
                    base.Visibility =  Visibility.Collapsed ;
                }



             




                public string EventName { get; set; }


                public FrameworkElement EventSource
                {
                    get { return (FrameworkElement)GetValue(EventSourceProperty); }
                    set { SetValue(EventSourceProperty, value); }
                }

                // Using a DependencyProperty as the backing store for EventSource.  This enables animation, styling, binding, etc...
                public static readonly DependencyProperty EventSourceProperty =
                    DependencyProperty.Register("EventSource", typeof(FrameworkElement), typeof(CommandBinding), new PropertyMetadata(null,
                        (dobj, arg) =>
                        {
                            CommandBinding obj = dobj as CommandBinding;
                            if (obj == null)
                            {
                                return;
                            }
                            if (obj.oldEventDispose != null)
                            {
                                obj.oldEventDispose.Dispose();
                            }
                            var nv = arg.NewValue;
                            if (nv != null)
                            {

                                obj.oldEventDispose = nv.BindEvent(
                                    obj.EventName,
                                    (o, a, en, ehType) =>
                                    {
                                        obj.ExecuteFromEvent(o, a, en, ehType);
                                    });

                            }

                        }


                        ));


                IDisposable oldEventDispose;

                public ICommand Command
                {
                    get { return (ICommand)GetValue(CommandProperty); }
                    set { SetValue(CommandProperty, value); }
                }

                // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
                public static readonly DependencyProperty CommandProperty =
                    DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandBinding), new PropertyMetadata(null));




                public Object Parameter
                {
                    get { return (Object)GetValue(ParameterProperty); }
                    set { SetValue(ParameterProperty, value); }
                }

                // Using a DependencyProperty as the backing store for Parameter.  This enables animation, styling, binding, etc...
                public static readonly DependencyProperty ParameterProperty =
                    DependencyProperty.Register("Parameter", typeof(Object), typeof(CommandBinding), new PropertyMetadata(null));



                public void ExecuteFromEvent(object sender, object eventArgs, string eventName, Type eventHandlerType)
                {
                    object vm = null;

                    var s = (sender as FrameworkElement);

                    if (Command == null)
                    {
                        return;
                    }
                    var cvm = Command as ICommandWithViewModel;
                    if (cvm != null)
                    {
                        vm = cvm.ViewModel;
                    }


                    var newe = EventCommandEventArgs.Create(Parameter, vm, sender, eventArgs, eventName, eventHandlerType);

                    if (Command.CanExecute(newe))
                    {
                        var spEventCommand = Command as EventCommandBase;
                        if (spEventCommand == null)
                        {
                            Command.Execute(newe);
                        }
                        else
                        {
                            spEventCommand.OnCommandExecute(newe);

                        }
                    }

                }



            }













        }


    }

}
