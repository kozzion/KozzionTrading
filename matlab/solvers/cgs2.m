function [x, hist, t] = cgs2(MV, b, x0, tol, kmax)
% x is the computed solution
% hist is the sequence norm of intermediate residuals 
% t is the computing time
%
% A the name of a file that defines the action of A on a vector
% b righthandside vector
% x0 initial guess (0 if not defined)
% kmax maximal number of iterations (100 if not defined)


%%% set defaults
if nargin<5, kmax=100; end
if nargin<4, tol=1.0e-8; end
if nargin<3, x0=0*b; end


t0    = clock; 
%%%
[n,m] = size(b); 
u     = zeros(n,1); 
w     = zeros(n,1);
sigma = 1; 
alpha = 1;

x    = x0;
%   r    = b(:,1) - feval(MV, x); 
r    = b(:,1) - (MV * x);
rt   = r; 
rho  = norm(r); 
hist = rho;
%%% scale tol for relative residual reduction
tol  = rho*tol;
k    = 0;
while k < kmax
    if (rho < tol) 
        break;
    end

    mu=rt'*r; beta=-(mu/sigma)/alpha;
    w=u-beta*w;
    u=r-beta*u;
    w=u-beta*w;
    %     c=feval(MV,w); 
    c = MV * w; 
    k=k+1;
    sigma=rt'*c; alpha=mu/sigma;
    u1=u-alpha*c;
    y=alpha*(u1+u); u=u1;
    x=x+y;
    %     c=feval(MV,y);
    c = MV * y;
    k=k+1;
    r=r-c;
    rho=norm(r); 
    hist=[hist,rho*ones(1,2)];

end
t=etime(clock,t0);
