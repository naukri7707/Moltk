namespace Naukri.Moltk.Fusion
{
    public class Subscription
    {
        internal Subscription(Node input, Node output)
        {
            this.input = input;
            this.output = output;
        }

        private readonly Node input;

        private readonly Node output;

        public void Start()
        {
            Node.Link(input, output);
        }

        public void Cancel()
        {
            Node.Unlink(input, output);
        }
    }
}
