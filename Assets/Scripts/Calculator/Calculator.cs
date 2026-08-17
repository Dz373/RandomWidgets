using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Calculator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textOutput;
    [SerializeField] private Transform buttons;

    private State state = State.input;

    private void Awake() {
        foreach (InputButton b in buttons.GetComponentsInChildren<InputButton>()) {
            b.button.onClick.AddListener(()=>ButtonInput(b.input));
        }
    }

    private void ButtonInput(string input) {
        if (input.Equals("=")) {
            List<string> postfix = InfixToPostfix(textOutput.text);
            double answer = EvalutatePostfix(postfix);

            textOutput.text = answer.ToString();
            state = State.answer;
        }
        else {
            if(state == State.answer) {
                ResetCalculator();
                state = State.input;
            }

            textOutput.text += input;
        }
    }

    static List<string> InfixToPostfix(string expr) {
        List<string> output = new List<string>();
        Stack<char> operators = new Stack<char>();
        for (int i = 0; i < expr.Length; i++) {
            char token = expr[i];

            if (char.IsDigit(token) || token == '.') {
                string num = "";
                while (i < expr.Length && (char.IsDigit(expr[i]) || expr[i] == '.'))
                    num += expr[i++];
                i--;

                output.Add(num);
            }

            else if (token == '(')
                operators.Push(token);

            else if (token == ')') {
                while (!(operators.Peek() == '('))
                    output.Add(operators.Pop().ToString());

                operators.Pop();
            }

            else {
                while (operators.Count > 0 && (Precedence(operators.Peek()) >= Precedence(token)))
                    output.Add(operators.Pop().ToString());
 
                operators.Push(token);
            }
        }

        while (operators.Count > 0)
            output.Add(operators.Pop().ToString());

        return output;
    }

    static double EvalutatePostfix(List<string> expr) {
        Stack<double> stack = new Stack<double>();
        foreach (string token in expr) {
            if (double.TryParse(token, out double num))
                stack.Push(num);

            else {
                double b = stack.Pop();
                double a = stack.Pop();
                switch (token) {
                    case "+":
                        stack.Push(a + b); break;
                    case "-":
                        stack.Push(a - b); break;
                    case "*":
                        stack.Push(a * b); break;
                    case "/":
                        stack.Push(a / b); break;
                    case "^":
                        stack.Push(Mathf.Pow((float)a, (float)b)); break;
                }
            }
        }

        return stack.Pop();
    }

    static int Precedence(char op) {
        if (op == '+' || op == '-') return 1;
        if (op == '*' || op == '/') return 2;
        if (op == '^') return 3;
        return 0;
    }

    public void ResetCalculator() {
        textOutput.text = "";
    }

    enum State {
        answer,
        input
    }
}
