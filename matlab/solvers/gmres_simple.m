function [solution, solutions, errors, norms] = gmres_simple(A, b, x0, kmax, chosen_solution)

  
  %%%
  x          = x0;
  r         = b - A * x;
  rho0      = norm(r); 
  rho       = rho0; 
  errors    = rho;
  solutions = x';
  norms     = norm(x);
  %%% scale tol for relative residual reduction
  
  H         = zeros(1,0);
  V         = r/rho; 
  gamma     = 1; 
  nu        = 1;
  for k = 1:kmax     
     [V,H]  = arnoldi_step_simple(A, V, H, -1);
     %%% compute the nul vector of H'
     gk     = (gamma * H(1:k,k)) / H(k+1,k); 
     gamma  = [gamma,-gk];
     %%% Compute the residual norm
     nu     = nu + gk'*gk; 
     rho    = rho0/sqrt(nu);
     errors = [errors,rho];
     % compute explicit residual every step
     k1      = size(H,2); 
     e      = zeros(k,1); 
     e(1,1) = 1;
     e      = [e;0];
     y      = H\e;
     x1     = V(:,1:k)*(rho0*y);     
     solutions = [solutions; x1'];
     norms    = [norms; norm(x1)];
     
  end
  %%% compute the approximate solution 
  solution  = solutions(chosen_solution,:);
  
return
