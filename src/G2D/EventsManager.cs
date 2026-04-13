namespace G2D;

public class EventsManager
{
    public delegate void EventAction<T>(in T e) where T : struct;

    public event EventAction<WindowShownEvent>? OnWindowShown;

    public event EventAction<WindowHiddenEvent>? OnWindowHidden;

    public event EventAction<WindowResizedEvent>? OnWindowResized;

    public event EventAction<WindowPixelSizeChangedEvent>? OnWindowPixelSizeChanged;

    public event EventAction<WindowMinimizedEvent>? OnWindowMinimized;

    public event EventAction<WindowMaximizedEvent>? OnWindowMaximized;

    public event EventAction<WindowRestoredEvent>? OnWindowRestored;

    public event EventAction<WindowMouseEnterEvent>? OnWindowMouseEnter;

    public event EventAction<WindowMouseLeaveEvent>? OnWindowMouseLeave;

    public event EventAction<WindowFocusGainedEvent>? OnWindowFocusGained;

    public event EventAction<WindowFocusLostEvent>? OnWindowFocusLost;

    public event EventAction<WindowEnterFullscreenEvent>? OnWindowEnterFullscreen;

    public event EventAction<WindowLeaveFullscreenEvent>? OnWindowLeaveFullscreen;

    public event EventAction<KeyDownEvent>? OnKeyDown;

    public event EventAction<KeyUpEvent>? OnKeyUp;

    public event EventAction<TextEditingEvent>? OnTextEditing;

    public event EventAction<TextInputEvent>? OnTextInput;

    public event EventAction<TextEditingCandidatesEvent>? OnTextEditingCandidates;

    public event EventAction<MouseMotionEvent>? OnMouseMotion;

    public event EventAction<MouseButtonDownEvent>? OnMouseButtonDown;

    public event EventAction<MouseButtonUpEvent>? OnMouseButtonUp;

    public event EventAction<MouseWheelEvent>? OnMouseWheel;

    internal void ProcessEvent(in Event e)
    {
        switch (e.EventType)
        {
            case EventType.WindowShown:
                OnWindowShown?.Invoke(e._windowShownEvent);
                break;
            case EventType.WindowHidden:
                OnWindowHidden?.Invoke(e._windowHiddenEvent);
                break;
            case EventType.WindowResized:
                OnWindowResized?.Invoke(e._windowResizedEvent);
                break;
            case EventType.WindowPixelSizeChanged:
                OnWindowPixelSizeChanged?.Invoke(e._windowPixelSizeChangedEvent);
                break;
            case EventType.WindowMinimized:
                OnWindowMinimized?.Invoke(e._windowMinimizedEvent);
                break;
            case EventType.WindowMaximized:
                OnWindowMaximized?.Invoke(e._windowMaximizedEvent);
                break;
            case EventType.WindowRestored:
                OnWindowRestored?.Invoke(e._windowRestoredEvent);
                break;
            case EventType.WindowMouseEnter:
                OnWindowMouseEnter?.Invoke(e._windowMouseEnterEvent);
                break;
            case EventType.WindowMouseLeave:
                OnWindowMouseLeave?.Invoke(e._windowMouseLeaveEvent);
                break;
            case EventType.WindowFocusGained:
                OnWindowFocusGained?.Invoke(e._windowFocusGainedEvent);
                break;
            case EventType.WindowFocusLost:
                OnWindowFocusLost?.Invoke(e._windowFocusLostEvent);
                break;
            case EventType.WindowEnterFullscreen:
                OnWindowEnterFullscreen?.Invoke(e._windowEnterFullscreenEvent);
                break;
            case EventType.WindowLeaveFullscreen:
                OnWindowLeaveFullscreen?.Invoke(e._windowLeaveFullscreenEvent);
                break;
            case EventType.KeyDown:
                OnKeyDown?.Invoke(e._keyDownEvent);
                break;
            case EventType.KeyUp:
                OnKeyUp?.Invoke(e._keyUpEvent);
                break;
            case EventType.TextEditing:
                OnTextEditing?.Invoke(e._textEditingEvent);
                break;
            case EventType.TextInput:
                OnTextInput?.Invoke(e._textInputEvent);
                break;
            case EventType.TextEditingCandidates:
                OnTextEditingCandidates?.Invoke(e._textEditingCandidatesEvent);
                break;
            case EventType.MouseMotion:
                OnMouseMotion?.Invoke(e._mouseMotionEvent);
                break;
            case EventType.MouseButtonDown:
                OnMouseButtonDown?.Invoke(e._mouseButtonDownEvent);
                break;
            case EventType.MouseButtonUp:
                OnMouseButtonUp?.Invoke(e._mouseButtonUpEvent);
                break;
            case EventType.MouseWheel:
                OnMouseWheel?.Invoke(e._mouseWheelEvent);
                break;

            default:
                throw new NotSupportedException("Unknown event type: " + e.EventType);
                break;
        }
    }
}