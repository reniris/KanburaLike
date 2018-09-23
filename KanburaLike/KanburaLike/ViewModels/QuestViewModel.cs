using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using KanburaLike.Models;
using Grabacr07.KanColleWrapper.Models;

namespace KanburaLike.ViewModels
{
	public class QuestViewModel : ViewModel
	{
		/* コマンド、プロパティの定義にはそれぞれ 
         * 
         *  lvcom   : ViewModelCommand
         *  lvcomn  : ViewModelCommand(CanExecute無)
         *  llcom   : ListenerCommand(パラメータ有のコマンド)
         *  llcomn  : ListenerCommand(パラメータ有のコマンド・CanExecute無)
         *  lprop   : 変更通知プロパティ(.NET4.5ではlpropn)
         *  
         * を使用してください。
         * 
         * Modelが十分にリッチであるならコマンドにこだわる必要はありません。
         * View側のコードビハインドを使用しないMVVMパターンの実装を行う場合でも、ViewModelにメソッドを定義し、
         * LivetCallMethodActionなどから直接メソッドを呼び出してください。
         * 
         * ViewModelのコマンドを呼び出せるLivetのすべてのビヘイビア・トリガー・アクションは
         * 同様に直接ViewModelのメソッドを呼び出し可能です。
         */

		/* ViewModelからViewを操作したい場合は、View側のコードビハインド無で処理を行いたい場合は
         * Messengerプロパティからメッセージ(各種InteractionMessage)を発信する事を検討してください。
         */

		/* Modelからの変更通知などの各種イベントを受け取る場合は、PropertyChangedEventListenerや
         * CollectionChangedEventListenerを使うと便利です。各種ListenerはViewModelに定義されている
         * CompositeDisposableプロパティ(LivetCompositeDisposable型)に格納しておく事でイベント解放を容易に行えます。
         * 
         * ReactiveExtensionsなどを併用する場合は、ReactiveExtensionsのCompositeDisposableを
         * ViewModelのCompositeDisposableプロパティに格納しておくのを推奨します。
         * 
         * LivetのWindowテンプレートではViewのウィンドウが閉じる際にDataContextDisposeActionが動作するようになっており、
         * ViewModelのDisposeが呼ばれCompositeDisposableプロパティに格納されたすべてのIDisposable型のインスタンスが解放されます。
         * 
         * ViewModelを使いまわしたい時などは、ViewからDataContextDisposeActionを取り除くか、発動のタイミングをずらす事で対応可能です。
         */

		/* UIDispatcherを操作する場合は、DispatcherHelperのメソッドを操作してください。
         * UIDispatcher自体はApp.xaml.csでインスタンスを確保してあります。
         * 
         * LivetのViewModelではプロパティ変更通知(RaisePropertyChanged)やDispatcherCollectionを使ったコレクション変更通知は
         * 自動的にUIDispatcher上での通知に変換されます。変更通知に際してUIDispatcherを操作する必要はありません。
         */


		#region Type 変更通知プロパティ

		private QuestType _Type;

		public QuestType Type
		{
			get { return this._Type; }
			set
			{
				if (this._Type != value)
				{
					this._Type = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Category 変更通知プロパティ

		private QuestCategory _Category;

		public QuestCategory Category
		{
			get { return this._Category; }
			set
			{
				if (this._Category != value)
				{
					this._Category = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region State 変更通知プロパティ

		private QuestState _State;

		public QuestState State
		{
			get { return this._State; }
			set
			{
				if (this._State != value)
				{
					this._State = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Progress 変更通知プロパティ

		private QuestProgress _Progress;

		public QuestProgress Progress
		{
			get { return this._Progress; }
			set
			{
				if (this._Progress != value)
				{
					this._Progress = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Title 変更通知プロパティ

		private string _Title;

		public string Title
		{
			get { return this._Title; }
			set
			{
				if (this._Title != value)
				{
					this._Title = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Detail 変更通知プロパティ

		private string _Detail;

		public string Detail
		{
			get { return this._Detail; }
			set
			{
				if (this._Detail != value)
				{
					this._Detail = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region IsUntaken 変更通知プロパティ

		private bool _IsUntaken;

		public bool IsUntaken
		{
			get { return this._IsUntaken; }
			set
			{
				if (this._IsUntaken != value)
				{
					this._IsUntaken = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion


		public QuestViewModel(Quest quest)
		{
			if (quest == null)
			{
				this.IsUntaken = true;
				this.Title = "(未取得の任務)";
			}
			else
			{
				this.IsUntaken = false;
				this.Type = quest.Type;
				this.Category = quest.Category;
				this.State = quest.State;
				this.Progress = quest.Progress;
				this.Title = quest.Title;
				this.Detail = quest.Detail;
			}
		}
	}
}

