namespace MeinTaschenrechner
{
    public class BasisRechner : BaseRechner
    {
        public BasisRechner() : base("Basis-Rechner")
        {
        }

        public override double Berechnen(string operation, params double[] werte)
        {
            if (!ValidiereEingaben(werte, 2))
            {
                throw new ArgumentException("Basis-Rechner benötigt mindestens 2 Werte");
            }

            double ergebnis;

            switch (operation.ToLower())
            {
                case "+":
                case "addition":
                    ergebnis = werte[0] + werte[1];
                    break;

                case "-":
                case "subtraktion":
                    ergebnis = werte[0] - werte[1];
                    break;

                case "*":
                case "multiplikation":
                    ergebnis = werte[0] * werte[1];
                    break;

                case "/":
                case "division":
                    if (werte[1] == 0)
                        throw new DivideByZeroException("Division durch Null nicht möglich!");
                    ergebnis = werte[0] / werte[1];
                    break;

                default:
                    throw new NotSupportedException($"Operation '{operation}' wird nicht unterstützt.");
            }

            // Berechnung automatisch speichern
            BerechnungSpeichern(operation, werte, ergebnis);

            return ergebnis;
        }
    }

}
