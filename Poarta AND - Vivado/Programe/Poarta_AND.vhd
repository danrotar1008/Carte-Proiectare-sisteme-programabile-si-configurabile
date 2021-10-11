----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 10.10.2021 06:56:16
-- Design Name: 
-- Module Name: Poarta_AND - Behavioral
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

entity Poarta_AND is
    Port ( In0 : in STD_LOGIC;
           In1 : in STD_LOGIC;
           Out0 : out STD_LOGIC);
end Poarta_AND;

architecture Behavioral of Poarta_AND is

begin

			poarta: process(In1, In0)
			begin
				if In1 = '0' then Out0 <= '0';
				else Out0 <= In0;
				end if;
			end process;

end Behavioral;
