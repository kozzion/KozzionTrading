%% layout: age, , man-woman[-1 1], right-left[-1 1]

items = [
    55, 1,  0.7;
    43,-1,  0.2;
    31, 1,  1.0;
    43, 1, -1.0;
    ]




respondent_count = 1000;
respondent_type_choise = [
    30,  1,  1.0; 
    30,  1,  1.0;
    55, -1, -1.0;
    55,  1, -1.0;
    ]

respondent_type_weigth = [
    0.5, 0.5, 0.5; 
    0.5, 0.5, 0.5;
    0.5, 0.5, 0.5;
    0.5, 0.5, 0.5;
    ]


respondent_thresholds = [
    0.5, 0.5, 0.5; 
    0.5, 0.5, 0.5;
    0.5, 0.5, 0.5;
    0.5, 0.5, 0.5;
    ];

%% simulate responses




preferrences = zeros(respondent_count, items_count)





%% recoup the original axes

correlations = cov(preferrences)
[COEFF,SCORE] = pca(correlations);


