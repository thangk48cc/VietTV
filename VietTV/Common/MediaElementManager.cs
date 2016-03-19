using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using SM.Media;
using SM.Media.MediaParser;
using SM.Media.Utility;

namespace VietTV.Common
{
    public class MediaElementManager : IMediaElementManager
    {
        readonly Func<MediaElement> _createMediaElement;
        readonly Action<MediaElement> _destroyMediaElement;
        readonly Dispatcher _dispatcher;
        MediaElement _mediaElement;
        int _sourceIsSet;

        public MediaElementManager(Dispatcher dispatcher, Func<MediaElement> createMediaElement, Action<MediaElement> destroyMediaElement)
        {
            _dispatcher = dispatcher;
            _createMediaElement = createMediaElement;
            _destroyMediaElement = destroyMediaElement;
        }

        #region IMediaElementManager Members

        public Task SetSourceAsync(IMediaStreamSource source)
        {
            return Dispatch(() =>
            {
                source.ValidateEvent(MediaStreamFsm.MediaEvent.MediaStreamSourceAssigned);

                var wasSet = Interlocked.Exchange(ref _sourceIsSet, 1);

                Debug.Assert(0 == wasSet);

                if (null != _mediaElement)
                {
                    UiThreadCleanup();

                    var mediaElement = _mediaElement;
                    _mediaElement = null;

                    _destroyMediaElement(mediaElement);
                }

                _mediaElement = _createMediaElement();

                if (null != _mediaElement)
                    _mediaElement.SetSource((MediaStreamSource)source);
                else
                    Debug.WriteLine("MediaElementManager.SetSourceAsync() null media element");
            });
        }

        public async Task CloseAsync()
        {
            var wasSet = Interlocked.CompareExchange(ref _sourceIsSet, 2, 1);

            if (0 != wasSet)
            {
                await Dispatch(() =>
                {
                    UiThreadCleanup();

                    var mediaElement = _mediaElement;
                    _mediaElement = null;

                    if (null != mediaElement)
                        _destroyMediaElement(mediaElement);
                })
                    .ConfigureAwait(false);
            }
        }

        #endregion

        Task Dispatch(Action action)
        {
            if (_dispatcher.CheckAccess())
            {
                action();

                return TplTaskExtensions.CompletedTask;
            }

            var tcs = new TaskCompletionSource<bool>();

            var dispatcherOperation = _dispatcher.BeginInvoke(() =>
            {
                try
                {
                    action();
                    tcs.TrySetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            });

            return tcs.Task;
        }

        void UiThreadCleanup()
        {
            var was2 = Interlocked.CompareExchange(ref _sourceIsSet, 3, 2);

            if (2 != was2 && 3 != was2)
                return;

            if (null == _mediaElement)
                return;

            var state = _mediaElement.CurrentState;

            if (MediaElementState.Closed != state && MediaElementState.Stopped != state)
                _mediaElement.Stop();

            state = _mediaElement.CurrentState;

            //if (MediaElementState.Closed == state || MediaElementState.Stopped == state)
            _mediaElement.Source = null;

            state = _mediaElement.CurrentState;

            if (MediaElementState.Closed == state || MediaElementState.Stopped == state)
            {
                var was3 = Interlocked.Exchange(ref _sourceIsSet, 0);

                Debug.Assert(3 == was3);
            }
        }
    }
}
