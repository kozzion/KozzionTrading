data = csvread('D:\GoogleDrive\BIG\data.csv');

sum(data(:,1))

is_man       = data(:,1) == 0
%%
proffesion_codes = unique(data(is_man, 2))
specialism_codes = unique(data(is_man, 3))

counts = zeros(numel(specialism_codes),4) 
years  = zeros(numel(specialism_codes),2)

names  = zeros(numel(specialism_codes),3)

for specialism_index = 1:numel(specialism_codes)
    
    specialism_code = specialism_codes(specialism_index)
    counts(specialism_index, 1) = specialism_code;
    counts(specialism_index, 2) = sum(data(:, 3) == specialism_code);
    counts(specialism_index, 3) = sum(data(is_man, 3) == specialism_code);
    counts(specialism_index, 4) = sum(data(~is_man, 3) == specialism_code);
    
    years(specialism_index, 1) = mean(data( data(data(is_man, 3) == specialism_code) == 1,4));
    years(specialism_index, 2) = mean(data( data(data(~is_man, 3) == specialism_code) == 1,4));
    
    names(specialism_index, 1) = specialism_code;
    names(specialism_index, 2) = mean(data( data(data(is_man, 3) == specialism_code) == 1,5));
    names(specialism_index, 3) = mean(data( data(data(~is_man, 3) == specialism_code) == 1,5));
end
counts
years
names
return
%%
counts_r = counts
%%
counts_r(:,2) = counts(:,3) ./ counts(:,2)
counts_r(:,3) = counts(:,4) ./ counts(:,2)




%% for 15 men
for specialism_index = 1:numel(specialism_codes)
    
    specialism_code = specialism_codes(specialism_index)
    years_15_m = data( data(data(is_man, 3) == specialism_code) == 1,4);
    years_15_f = data( data(data(~is_man, 3) == specialism_code) == 1,4);
    figure()
    histogram(years_15_m)
    hold on
    histogram(years_15_f)
    hold off
    pause()

end





%% for 15 men
   figure()
    years_total = data(:,4);
    histogram(years_total(years_total > 2000))
    %%
   figure()
    years_total = data(:,4);
    histogram(years_total)