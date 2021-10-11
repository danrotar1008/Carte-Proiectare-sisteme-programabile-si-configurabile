----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 10.10.2021 17:02:47
-- Design Name: 
-- Module Name: mux8x1 - arch_mux8x1
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

entity mux8x1 is
    Port ( in_mux : in STD_LOGIC_VECTOR (7 downto 0);
           in_comanda : in STD_LOGIC_VECTOR (2 downto 0);
           ies_mux : out STD_LOGIC);
end mux8x1;

architecture arch_mux8x1 of mux8x1 is

begin

   process (in_mux) is
   begin
      case in_comanda is
         when "000"  => ies_mux <= in_mux(0);
         when "001"  => ies_mux <= in_mux(1);
         when "010"  => ies_mux <= in_mux(2);
         when "011"  => ies_mux <= in_mux(3);
         when "100"  => ies_mux <= in_mux(4);
         when "101"  => ies_mux <= in_mux(5);
         when "110"  => ies_mux <= in_mux(6);
         when others => ies_mux <= in_mux(7);		
      end case;
   end process;

end arch_mux8x1;
