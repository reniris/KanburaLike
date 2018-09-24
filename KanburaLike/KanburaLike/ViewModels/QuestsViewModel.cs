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
using Grabacr07.KanColleWrapper;
using KanburaLike.Models.Settings;

namespace KanburaLike.ViewModels
{
	public class QuestsViewModel : ViewModel
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

		public QuestsSetting Setting { get; }

		public int Count => Current != null ? Current.Count() : 0;

		#region Current 変更通知プロパティ

		private QuestViewModel[] _Current;

		public QuestViewModel[] Current
		{
			get { return this._Current; }
			set
			{
				if (this._Current != value)
				{
					this._Current = value;
					this.RaisePropertyChanged();
					this.RaisePropertyChanged(nameof(Count));
				}
			}
		}

		#endregion

		#region Quests 変更通知プロパティ

		private QuestViewModel[] _Quests;

		public QuestViewModel[] Quests
		{
			get { return this._Quests; }
			set
			{
				if (this._Quests != value)
				{
					this._Quests = value;
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

		#region IsEmpty 変更通知プロパティ

		private bool _IsEmpty;

		public bool IsEmpty
		{
			get { return this._IsEmpty; }
			set
			{
				if (this._IsEmpty != value)
				{
					this._IsEmpty = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		public const string Untaken = "任務画面の全ページにアクセスしてください。";

		public QuestsViewModel()
		{
			Setting = SettingsHost.Cache<QuestsSetting>(k => new QuestsSetting(), nameof(QuestsSetting));

			var quests = KanColleClient.Current.Homeport.Quests;

			this.IsUntaken = quests.IsUntaken;
			this.Quests = quests.All.Select(x => new QuestViewModel(x)).ToArray();
			UpdateCurrent(quests);
			this.IsEmpty = quests.IsEmpty;

			this.CompositeDisposable.Add(new PropertyChangedEventListener(quests)
			{
				{ nameof(quests.IsUntaken), (sender, args) => this.IsUntaken = quests.IsUntaken },
				{ nameof(quests.All), (sender, args) => this.Quests = quests.All.Select(x => new QuestViewModel(x)).ToArray() },
				{ nameof(quests.Current), (sender, args) => UpdateCurrent(quests) },
				{ nameof(quests.IsEmpty), (sender, args) => this.IsEmpty = quests.IsEmpty }
			});
		}

		private void UpdateCurrent(Quests quests)
		{
			if(this.IsUntaken == true)
			{
				//Setting.QuestsIDから読み取ってなんかする
			}

			this.Current = quests.Current.Select(x => new QuestViewModel(x)).ToArray();
			this.Setting.QuestsID = this.Current.Select(x => x.Id).ToArray();
		}
	}
}
