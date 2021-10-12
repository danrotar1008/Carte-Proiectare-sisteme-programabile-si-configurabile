----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 12.10.2021 14:27:21
-- Design Name: 
-- Module Name: selector - arh_selector
-- Project Name: 
-- Target Devices: 
-- Tool Versions: 
-- Description: 
-- 
-- Dependencies: 
-- 
-- Revision:
-- Revision 0.01 - File Created
-- Additional Comments:
-- 
----------------------------------------------------------------------------------


library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity selector is
    Port ( intrare : in STD_LOGIC_VECTOR (3 downto 0);
           iesire : out STD_LOGIC);
end selector;

architecture arh_selector of selector is

begin

    iesire <= '1' when intrare = "1000" else '0';

end arh_selector;
