pragma solidity >=0.4.0 <0.6.0;

contract test {
    int _multiplier;

    constructor(int multiplier) public{
        _multiplier = multiplier;
    }

    function multiply(int val) public returns (int d) {
        return val * _multiplier;    
    }
}