----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 10.10.2021 11:06:46
-- Design Name: 
-- Module Name: Poarta_AND_TB - Behavioral
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

entity Poarta_AND_TB is
--  Port ( );
end Poarta_AND_TB;

architecture Behavioral of Poarta_AND_TB is

    -- Component Declaration for the Unit Under Test (UUT)
 
    COMPONENT Poarta_AND
    PORT(
         In0 : IN  std_logic;
         In1 : IN  std_logic;
         Out0 : OUT  std_logic
        );
    END COMPONENT;
    

   --Inputs
   signal In0 : std_logic := '0';
   signal In1 : std_logic := '0';

 	--Outputs
   signal Out0 : std_logic;

begin

	-- Instantiate the Unit Under Test (UUT)
   uut: Poarta_AND PORT MAP (
          In0 => In0,
          In1 => In1,
          Out0 => Out0
        );

   -- Stimulus process
   stim_proc: process
   begin		
      -- hold reset state for 100 ns.
      wait for 100 ns;	

      -- insert stimulus here 
		In1 <= '0';
		In0 <= '0';

		wait for 100 ns;
		In1 <= '0';
		In0 <= '1';

		wait for 100 ns;
		In1 <= '1';
		In0 <= '0';

		wait for 100 ns;
		In1 <= '1';
		In0 <= '1';

		wait for 100 ns;
		In1 <= '0';
		In0 <= '0';
      wait;
   end process;

end Behavioral;
