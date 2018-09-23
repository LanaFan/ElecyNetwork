using System.Collections.Generic;

namespace ElecyServer
{
    public class BaseUpdate<T>
    {
        protected Dictionary<int, UpdateContainer<T>> updateLibruary;
        protected T currentValue;
        protected int currentIndex;
        private object _expectant;

        public BaseUpdate(T value)
        {
            updateLibruary.Add(1, new UpdateContainer<T>(value));
            currentIndex = 1;
            currentValue = value;
        }

        public void SetUpdate(T Value, int Index)
        {
            lock (_expectant)
            {
                if (updateLibruary.ContainsKey(Index))
                    return;
                if (currentIndex < Index)
                {
                    if (updateLibruary.Count > 20)
                    {
                        if (updateLibruary.TryGetValue(1, out UpdateContainer<T> startBuffer))
                        {
                            updateLibruary.Clear();
                            updateLibruary.Add(1, startBuffer);
                        }
                        else
                            Global.serverForm.Debug("There is no start position in memory");
                    }
                    currentIndex = Index;
                    currentValue = Value;
                    updateLibruary.Add(currentIndex, new UpdateContainer<T>(currentValue));
                }
                else
                {
                    updateLibruary.Add(Index, new UpdateContainer<T>(Value));
                }
            }
        }

        public bool GetPosition(out UpdateContainer<T> update, out int index)
        {
            lock (_expectant)
            {
                index = currentIndex;
                if (updateLibruary.TryGetValue(currentIndex, out update))
                    if (!update.sent)
                    {
                        update.Sent(); 
                        return true;
                    }
                return false;
            }
        }

        public void UdpateStepBack(int Index)
        {
            lock (_expectant)
            {
                if (updateLibruary.TryGetValue(1, out UpdateContainer<T> buffer))
                {
                    if (updateLibruary.TryGetValue(Index, out UpdateContainer<T> stepBackBuffer))
                    {
                        currentIndex = Index;
                        currentValue = stepBackBuffer.value;
                        updateLibruary.Clear();
                        updateLibruary.Add(1, buffer);
                        updateLibruary.Add(Index, stepBackBuffer);
                    }
                    else
                    {
                        Global.serverForm.Debug("There is no stepback point");
                    }
                }
                else
                {
                    Global.serverForm.Debug("There is no start point");
                }
            }
        }
    }

    public struct UpdateContainer<T>
    {
        public T value;
        public bool sent;
        public bool recieve;

        public UpdateContainer(T value)
        {
            this.value = value;
            sent = false;
            recieve = false;
        }

        public void Sent()
        {
            sent = true;
        }

        public void Recieve()
        {
            recieve = true;
        }
    }
}
