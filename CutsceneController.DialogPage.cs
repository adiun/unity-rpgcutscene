class DialogPage {
    private static readonly float _readingWps = 3;
    private readonly string _text;
    private float _readingTime;

    public string Text { get { return _text; } }
    public float ReadingTime { get { return _readingTime; }}

    public DialogPage(string text) {
        _text = text;
        _readingTime = ComputeReadingTime(_text);
    }

    private static float ComputeReadingTime(string text) {
        var numWords = text.Split(' ').Length;
        return (numWords / _readingWps) + 1;
    }
}