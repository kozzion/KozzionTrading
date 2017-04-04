function [v,h]=Orth(V,w,kappa)
%  w=[V,v]*h; with v such that v'*V=0;
%
%  kappa=0: classical Gram-Schmidt
%  kappa>0: repeated Gram-Schmidt with DGKS stopping criterion
%           repeat if tan(angle)<kappa
%  kappa<0: modified Gram-Schmidt
%

global REPEATED

   if nargin==2, kappa=0; end
   [n,k]=size(V);
   if (k>=n), kappa=0; end
   if (k==0), h=norm(w,2); v=w/h; REPEATED=[]; return, end
   if ~exist('REPEATED'), REPEATED=[]; end

   zero=k*k*n*norm(w)*eps; %%% numerical zero 
   v=w;
   if kappa<0 %%% modified Gram-Schmidt
      for j=1:k
        h(j,1)=V(:,j)'*v;
        v=v-V(:,j)*h(j,1);
      end
      rho=norm(v,2);
   else %%% repeated Gram-Schmidt
      h=V'*v;
      v=v-V*h;
      rho=norm(v,2);
      mu=norm(h,2);        t=0;
      %%% Daniel Gragg Kaufman Stewart update criterion
      %%% if kappa==0, classical Gram-Schmidt
      while (rho<kappa*mu & rho>zero)
         g=V'*v; v=v-V*g;
         rho=norm(v,2); mu=norm(g,2);
         h=h+g;           t=t+1;
      end,                REPEATED(1,size(V,2))=t;
   end
   if rho<2*zero
      %%% if full dimension then no expansion
      if (k>=n), v=zeros(n,0); return, end
      %%% if W in span(V) expand with random vector
      v=Orth(V,rand(n,1),kappa); 
      h=[h;0]; 
   else
      h=[h;rho];
      v=v/rho;
   end
return
