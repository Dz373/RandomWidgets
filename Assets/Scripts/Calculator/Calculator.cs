using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Calculator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textOutput;
    [SerializeField] private Transform buttons;

    private List<string> expression = new List<string>();
    private List<int> digits = new List<int>();

    private State state = State.input;

    private void Awake() {
        foreach (InputButton b in buttons.GetComponentsInChildren<InputButton>()) {
            b.button.onClick.AddListener(()=>ButtonInput(b.input));
        }
    }

    private void ButtonInput(string input) {
        if (input.Equals("=")) {
            expression.Add(ConvertDigits());

            int answer = EvaluateInfix(expression);

            textOutput.text = answer.ToString();
            state = State.answer;
        }
        else {
            if(state == State.answer) {
                ClearCalculator();
                state = State.input;
            }

            textOutput.text += input;

            if (IsNumber(input)) {
                digits.Add(int.Parse(input));
            }
            else {
                expression.Add(ConvertDigits());
                expression.Add(input);
                digits.Clear();
            }
        }
    }

    static int EvaluateInfix(List<string> arr) {
        Stack<int> values = new Stack<int>();
        Stack<string> ops = new Stack<string>();

        int n = arr.Count;
        for (int i = 0; i < n; i++) {
            string token = arr[i];

            if (IsNumber(token)) {
                values.Push(int.Parse(token));
            }

            else {
                while (ops.Count > 0 && ((Precedence(ops.Peek()) > Precedence(token))
                        || (Precedence(ops.Peek()) == Precedence(token)
                        && !IsRightAssociative(token)))) {

                    int val2 = values.Pop();
                    int val1 = values.Pop();
                    string op = ops.Pop();
                    values.Push(ApplyOperation(val1, val2, op));
                }
                ops.Push(token);
            }
        }

        while (ops.Count > 0) {
            int val2 = values.Pop();
            int val1 = values.Pop();
            string op = ops.Pop();
            values.Push(ApplyOperation(val1, val2, op));
        }

        return values.Pop();
    }

    static int ApplyOperation(int a, int b, string op) {
        if (op == "+") return a + b;
        if (op == "-") return a - b;
        if (op == "*") return a * b;
        if (op == "/") {
            if (a * b < 0 && a % b != 0)
                return (a / b) - 1;
            return a / b;
        }
        if (op == "^")
            return (int)Mathf.Pow(a, b);

        return 0;
    }

    static int Precedence(string op) {
        if (op == "+" || op == "-") return 1;
        if (op == "*" || op == "/") return 2;
        if (op == "^") return 3;
        return 0;
    }

    static bool IsRightAssociative(string op) {
        return op == "^";
    }

    static bool IsNumber(string token) {
        if (string.IsNullOrEmpty(token))
            return false;

        int start = (token[0] == '-') ? 1 : 0;
        if (start == 1 && token.Length == 1)
            return false;

        for (int i = start; i < token.Length; i++) {
            if (!char.IsDigit(token[i]))
                return false;
        }
        return true;
    }

    private string ConvertDigits() {
        int newNum = 0;
        for (int i = 0; i < digits.Count; i++) {
            newNum += digits[digits.Count - 1 - i] * (int)Mathf.Pow(10, i);
        }

        digits.Clear();
        return newNum.ToString();
    }

    public void ClearCalculator() {
        expression.Clear();
        digits.Clear();
        textOutput.text = "";
    }

    enum State {
        answer,
        input
    }
}
