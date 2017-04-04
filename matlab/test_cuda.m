gpuDevice
size = 12000;

A_cpu = ones(20, 1) * 2;
A_gpu = gpuArray(A_cpu);

tic;
B_cpu = exp(A_cpu)
toc
tic;
B_gpu = exp(A_gpu)
toc






