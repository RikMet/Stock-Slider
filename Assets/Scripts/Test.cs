using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    [SerializeField] private TextMeshProUGUI _sliderText;

    [SerializeField] private TextMeshProUGUI _numberOfStocksText;

    [SerializeField] private TextMeshProUGUI _valueOfStocksText;

    [SerializeField] private TextMeshProUGUI _cashText;

    [SerializeField] private TextMeshProUGUI _totalIncreaseText;

    [SerializeField] private TextMeshProUGUI _brokerageText;

    private double _cash = 20400;
    private double _brokerage = 0;
    private int _numberOfStocks = 5400;
    private double _valueOfStocks = 218754;
    private double _stockPrice = 40.51;
    private double _originalBalance = 239154;
    private double _totalIncrease = 0;
    private double __previousTriggerPrice = 50.5;
    private int _numberOfStocksInTransaction = 200;
    public enum InvestorAction
    {
        Hodl = 0,
        Buy = 1,
        Sell = 2
    }
    //void calculateValues(var inputAction)
    //{

    //}


    // Start is called before the first frame update
    void Start()
    {
        _slider.onValueChanged.AddListener((v) =>
        {
            _sliderText.text = v.ToString("0.00");
            _stockPrice = v;
            if (BuyTriggered(__previousTriggerPrice, _stockPrice))
            {
                CalculateValues(InvestorAction.Buy);
            }

            if (SellTriggered(__previousTriggerPrice, _stockPrice))
            {
                CalculateValues(InvestorAction.Sell);
            }

            UpdateLabels();
        });
        


        void CalculateValues(InvestorAction action)
        {
            if(action == InvestorAction.Buy)

            {
                _numberOfStocks += _numberOfStocksInTransaction;
                _cash -= (_numberOfStocksInTransaction * _stockPrice);
            }

            if (action == InvestorAction.Sell)

            {
                _numberOfStocks -=_numberOfStocksInTransaction;              
                _cash += (_numberOfStocksInTransaction * _stockPrice);
            }

            _brokerage += 29;
            _valueOfStocks = _stockPrice * _numberOfStocks;
            _totalIncrease = CalculateAndReturnIncreasePercentage();
        }

        void UpdateLabels()
        {
            _numberOfStocksText.text = _numberOfStocks.ToString();
            _valueOfStocksText.text = _valueOfStocks.ToString("0.00");
            _cashText.text = _cash.ToString("0.00");
            _brokerageText.text = _brokerage.ToString("0.00");
            _totalIncreaseText.text = DoubleToPercentageString(_totalIncrease);
        }

        double CalculateAndReturnIncreasePercentage()
        {
            double currentSum = _valueOfStocks + _cash - _brokerage;
            return CalculateChangePercentage(_originalBalance, currentSum);
            
        }
        double CalculateChangePercentage(double previous, double current)
        {
            if (previous == 0)
                throw new InvalidOperationException();

            var change = current - previous;
            return (double)change / previous;
        }

        bool BuyTriggered(double prevTriggerPrice, double stockPrice)
        {
            if ((prevTriggerPrice - stockPrice) >= 10)
            {
                __previousTriggerPrice = stockPrice; 
                return true;
            }
            
            return false;
        }
        bool SellTriggered(double prevTriggerPrice, double stockPrice)
        {
            if (prevTriggerPrice - stockPrice <= -10)
            {
                __previousTriggerPrice = stockPrice;
                return true;
            }
            return false;
        }

        

        string DoubleToPercentageString(double d)
        {
            return "%" + (Math.Round(d, 2) * 100).ToString();
        }
    }

}
