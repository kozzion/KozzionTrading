table = csvread('D:\GoogleDrive\TestData\Trading\Spreads.csv');

table(:,2) = table(:,2)/ sum(table(:,2));
plot(table(:,1), table(:,2))
title('spreads')
ylabel('spread occurance')
xlabel('spread size')