# Vending-Machine

.NET Requirements

In this exercise you will build the brains of a vending machine. It will accept money, make change and dispense products. All the things that you might expect a vending machine to accomplish.

EG-1 - Accept Coins

As a vendor

I want customers to select products

So that I can give them an incentive to put money in the machine

There are three products: cola for $1.00, chips for $0.50, and candy for $0.65. When the respective button is pressed and enough money has been inserted, the product is dispensed and the machine displays THANK YOU. If the display is checked again, it will display INSERT COIN and the current amount will be set to $0.00. If there is not enough money inserted then the machine displays PRICE and the price of the item and subsequent checks of the display will display either INSERT COIN or the current amount as appropriate.

NOTE: The temptation here will be to create Coin objects that know their value. However, this is not how a real vending machine works. Instead, it identifies coins by their weight and size and then assigns a value to what was inserted. You will need to do something similar. This can be simulated using strings, constants, enums, symbols, or something of that nature.

EG-2 - Select Product

As a vendor

I want customers to receive correct change

So that they will use the vending machine again

When a product is selected that costs less than the amount of money in the machine, then the remaining amount is placed in the coin return.

EG-3 - Make Change

As a customer

I want to have my money returned

So that I can change my mind about buying stuff from the vending machine

When the return coins button is pressed, the money the customer has placed in the machine is returned and the display shows INSERT COIN.
