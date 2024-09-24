CREATE TABLE WishlistItem (
    WishlistItemId INT PRIMARY KEY IDENTITY(1,1),
    RewardId INT NOT NULL,
    Quantity INT NOT NULL,
    AccId INT NOT NULL,
    FOREIGN KEY (RewardId) REFERENCES Reward(reward_id),
    FOREIGN KEY (AccId) REFERENCES Account(acc_id)
);