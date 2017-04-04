function [V, H] = arnoldi_step_simple(A, V, H, kappa)

   if (nargin < 4)
       kappa = 0.2; 
   end
   k     = size(V,2);
   v     = A * V(:,k);
   [v,h] = Orth(V, v, kappa);
   V     = [V, v]; 
   H     = [[H;zeros(1,k-1)],h];

return
