A = zeros(5,5);
x0 = zeros(5,1);
b = zeros(5,1);

A(1,1) = 1;
A(2,2) = 2;
A(3,3) = 3;
A(4,4) = 4;
A(5,5) = 5;


b(1) = 1;
b(2) = 2;
b(3) = 3;
b(4) = 4;
b(5) = 5;
A
x0
b

x = A\b

[~, solutions, ~, ~] = gmres_simple(A, b, x0, 4, 1)